using System.Collections;
using Deluz;
using UnityEngine;

public class CameraController : MonoBehaviour{
    public static CameraController instance;

    //[SerializeField] CameraFollowType follow_type = CameraFollowType.LockY;
    [SerializeField] private Transform target;
    [SerializeField] private float 
        smooth_speed, 
        original_size;
    [SerializeField] Vector2 
        x_bounds, // x is left, y is right
        y_bounds, // x is bot, y is top
        original_x_bounds,
        original_y_bounds;
    [SerializeField] private Vector3 
        offset,
        original_offset;
    [SerializeField] private bool regulate;
    [SerializeField] Camera cam;

    Coroutine
        follow_state,
        zoom_state,
        x_offset_state,
        y_offset_state,
        x_bounds_state,
        y_bounds_state,
        shake_state;

    void Awake(){
        instance = this;
        original_offset = offset;
        original_size = cam.orthographicSize;
        original_x_bounds = x_bounds;
        original_y_bounds = y_bounds;
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
        regulated.x = pos.x < x_bounds.x ? x_bounds.x : (pos.x > x_bounds.y ? x_bounds.y : pos.x);
        regulated.y = pos.y < y_bounds.x ? y_bounds.x : (pos.y > y_bounds.y ? y_bounds.y : pos.y);
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





    // lerpers and modifiers:
    public void lerp_offset(float? x, float? y, float time){
        state_swtich(ref y_offset_state, Calc.lerp_value(val => offset.y = val, offset.y, y.GetValueOrDefault(offset.y), time));
        state_swtich(ref x_offset_state, Calc.lerp_value(val => offset.x = val, offset.x, x.GetValueOrDefault(offset.x), time));
    }
    public void reset_offset(float time) => lerp_offset(original_offset.x, original_offset.y, time);
    public void lerp_zoom(float size, float time) => state_swtich(ref zoom_state, Calc.lerp_value(val => cam.orthographicSize = val, cam.orthographicSize, size, time));
    public void reset_zoom(float time) => lerp_zoom(original_size, time);
    public void ZoomIn() => cam.orthographicSize--;
    public void ZoomOut() => cam.orthographicSize++;
    public void lerp_regulators(Vector2? _x_bounds, Vector2? _y_bounds, float time){
        state_swtich(ref x_bounds_state, Calc.lerp_vector2(val => x_bounds=val, x_bounds, _x_bounds.GetValueOrDefault(x_bounds), time));
        state_swtich(ref y_bounds_state, Calc.lerp_vector2(val => y_bounds=val, y_bounds, _y_bounds.GetValueOrDefault(y_bounds), time));
    }
    public void reset_regulators(float time) => lerp_regulators(original_x_bounds,original_y_bounds,time);
}

public enum CameraFollowType{
    LockY,
    LockX,
    Unlocked
}