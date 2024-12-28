using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class CameraController : MonoBehaviour{
    public static CameraController instance;

    //[SerializeField] CameraFollowType follow_type = CameraFollowType.LockY;
    [SerializeField] private Transform target;
    [SerializeField, Range(0,5)] private float smooth_speed = 3.75f;
    [SerializeField] private Vector3 offset;
    [SerializeField] private bool regulate;
    [SerializeField] private float left_x_bound;
    [SerializeField] private float right_x_bound;
    [SerializeField] private float bot_y_bound;
    [SerializeField] private float top_y_bound;
    [SerializeField] Camera cam;
    public float original_size;
    public Vector3 original_offset;

    Coroutine
        follow_state,
        zoom_state,
        swivel_state,
        shake_state;

    void Awake(){
        instance = this;
        original_offset = offset;
        original_size = cam.orthographicSize;
        snap_to_target();
        start_follow_state();
    }

    public void regulate_in_bounds(bool x) => regulate = x;

    // external functions
    public void set_offset(Vector3 _offset, bool _override_original_offset){
        offset = _offset;
        original_offset = _override_original_offset == true? _offset : original_offset;
    }
    public void set_target(Transform _target) => target = _target;
    public void zoom_in_state(float size, float speed)      => state_swtich(ref zoom_state, zoom_in(size, speed));
    public void zoom_out_state(float size, float speed)     => state_swtich(ref zoom_state, zoom_out(size, speed));
    public void reset_zoom_state(float speed)               => state_swtich(ref zoom_state, reset_zoom(speed));
    public void move_vertical_state(float y_pos, float speed)     => state_swtich(ref swivel_state, move_vertical(y_pos, speed));
    public void move_horizontal_state(float x_pos, float speed)   => state_swtich(ref swivel_state, move_horizontal(x_pos, speed));
    public void reset_offset_state(float speed)             => state_swtich(ref swivel_state, reset_offset(speed));
    public void shake_camera(float time, float amount)      => state_swtich(ref shake_state, camera_shake(time, amount));

    // state switchers:
    void state_swtich(ref Coroutine state, IEnumerator n_state){
        if(state != null)
            StopCoroutine(state);
        state = StartCoroutine(n_state);
    }





    // follow states:
    public void snap_to_target() => transform.position = new Vector3(target.position.x + offset.x, offset.y, offset.z);
    public void stop_follow_state() => StopCoroutine(follow_state);
    public void start_follow_state() => state_swtich(ref follow_state, follow_locked_y());
    //public void start_follow_state(CameraFollowType? type = null){
    //    follow_type = type!=null? type.Value : follow_type;
    //    switch(follow_type){
    //        case CameraFollowType.LockY:        state_swtich(ref follow_state, follow_locked_y()); break;
    //        case CameraFollowType.Unlocked:    state_swtich(ref follow_state, follow_unlocked()); break;
    //        case CameraFollowType.LockX:        state_swtich(ref follow_state, follow_locked_x()); break;
    //    }
    //}
    IEnumerator follow_locked_y(){
        while(true){
            Vector3 desired_pos = new Vector3(target.position.x + offset.x, offset.y, offset.z);
            // may want to swap this to Vector3.SmoothDamp();
            Vector3 smoothed_pos = Vector3.Lerp(transform.position, desired_pos, smooth_speed * Time.deltaTime);
            transform.position = regulate_position(smoothed_pos);            
            yield return new WaitForEndOfFrame();
        }
    }
    IEnumerator follow_locked_x(){
        while(true){
            Vector3 desired_pos = new Vector3(offset.x, offset.y + target.position.y, offset.z);
            // may want to swap this to Vector3.SmoothDamp();
            Vector3 smoothed_pos = Vector3.Lerp(transform.position, desired_pos, smooth_speed * Time.deltaTime);
            transform.position = regulate_position(smoothed_pos);            
            yield return new WaitForEndOfFrame();
        }
    }
    IEnumerator follow_unlocked(){
        while(true){
            Vector3 desired_pos = new Vector3(offset.x + target.position.x, offset.y + target.position.y, offset.z);
            // may want to swap this to Vector3.SmoothDamp();
            Vector3 smoothed_pos = Vector3.Lerp(transform.position, desired_pos, smooth_speed * Time.deltaTime);
            transform.position = regulate_position(smoothed_pos);            
            yield return new WaitForEndOfFrame();
        }
    }
    Vector3 regulate_position(in Vector3 pos){
        if(regulate == false)
            return pos;
        Vector3 regulated = new Vector3();
        regulated.x = pos.x < left_x_bound ? left_x_bound : (pos.x > right_x_bound ? right_x_bound : pos.x);
        regulated.y = pos.y < bot_y_bound ? bot_y_bound : (pos.y > top_y_bound ? top_y_bound : pos.y);
        regulated.z = pos.z;
        return regulated;

    }






    // camera shake
    IEnumerator camera_shake(float time, float amount){
        float timer = 0;
        while(timer < time){
            // Generate a random shake value based on orthographic size and amount
            float shake = (Random.Range(0f, 11f) - 5) * cam.orthographicSize * amount / 10;

            // Apply shake to one or both axes (adjust as needed)
            Vector3 desired_pos = new Vector3(shake + transform.position.x, shake + transform.position.y, offset.z);

            // Smoothly transition to the desired position
            Vector3 smoothed_pos = Vector3.Lerp(transform.position, desired_pos, smooth_speed * Time.deltaTime);

            // Add the smoothed shake to the current position
            transform.position += smoothed_pos - transform.position;
            // Increment timer
            timer += Time.deltaTime;
            //state_swtich(ref follow_state, follow());
            yield return new WaitForFixedUpdate();
        }
        yield break;
    }





    // offset modifiers:
    IEnumerator move_vertical(float y_pos, float speed){
        while(Mathf.Abs(offset.y - y_pos) > 0.1){
            offset.y = Mathf.MoveTowards(offset.y, y_pos, Time.deltaTime * speed);
            yield return new WaitForEndOfFrame();
        }
        offset.y = y_pos;
        yield break;
    }
    IEnumerator move_horizontal(float x_pos, float speed){
        while(Mathf.Abs(offset.x - x_pos) > 0.1){
            offset.x = Mathf.MoveTowards(offset.x, x_pos, Time.deltaTime * speed);
            yield return null;
        }
        offset.x = x_pos;
        yield break;       
    }
    IEnumerator reset_offset(float speed){
        // determine if we are too high or too low.
        float difference = offset.y - original_offset.y;
        
        if(difference >= 0)
            state_swtich(ref swivel_state,move_vertical(original_offset.y, speed));
        else
            state_swtich(ref swivel_state,move_vertical(original_offset.y, speed));
        offset.y = original_offset.y;
        yield break;
    }





    // camera size modifiers:
    IEnumerator zoom_out(float size, float speed){
        while(cam.orthographicSize < size){
            cam.orthographicSize += Time.deltaTime * speed;
            yield return new WaitForEndOfFrame();
        }
        cam.orthographicSize = size;
        yield break;
    }
    IEnumerator zoom_in(float size, float speed){
        while(cam.orthographicSize > size){
            cam.orthographicSize -= Time.deltaTime * speed;
            yield return new WaitForEndOfFrame();
        }
        cam.orthographicSize = size;
    }
    IEnumerator reset_zoom(float speed){
        // determine if we are too high or too low.
        float difference = cam.orthographicSize - original_size;
        
        if(difference >= 0)
            state_swtich(ref zoom_state,zoom_in(original_size, speed));
        else
            state_swtich(ref zoom_state,zoom_out(original_size, speed));
        
        yield break;
    }
    public void ZoomIn()    =>    cam.orthographicSize--;
    public void ZoomOut()   =>    cam.orthographicSize++;
}

public enum CameraFollowType{
    LockY,
    LockX,
    Unlocked
}