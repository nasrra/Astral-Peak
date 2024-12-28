using System.Collections;
using Deluz;
using UnityEngine;

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
        x_offset_state,
        y_offset_state,
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
    public void lerp_offset(float? x, float? y, float time){
        state_swtich(ref y_offset_state, Calc.lerp_value(val => offset.y = val, offset.y, y.GetValueOrDefault(offset.y), time));
        state_swtich(ref x_offset_state, Calc.lerp_value(val => offset.x = val, offset.x, x.GetValueOrDefault(offset.x), time));
    }
    public void reset_offset(float time) => lerp_offset(original_offset.x, original_offset.y, time);
    public void lerp_zoom(float size, float time) =>
        state_swtich(ref zoom_state, Calc.lerp_value(val => cam.orthographicSize = val, cam.orthographicSize, size, time));
    public void reset_zoom(float time) => lerp_zoom(original_size, time);
    public void ZoomIn()    =>    cam.orthographicSize--;
    public void ZoomOut()   =>    cam.orthographicSize++;
}

public enum CameraFollowType{
    LockY,
    LockX,
    Unlocked
}