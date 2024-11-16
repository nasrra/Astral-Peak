using System.Collections;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraController : MonoBehaviour{
        
    public static CameraController instance;

    [SerializeField] private Transform target;
    [SerializeField, Range(0,5)] private float smooth_speed = 3.75f;
    [SerializeField] private Vector3 offset;
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
        swivel_state;

    void Awake(){
        instance = this;
        original_offset = offset;
        original_size = cam.orthographicSize;
        follow_state = StartCoroutine(follow());
        //test = StartCoroutine(test_zoom());
    }

    // external functions
    public void zoom_in_state(float size, float speed)      => state_swtich(ref zoom_state, zoom_in(size, speed));
    public void zoom_out_state(float size, float speed)     => state_swtich(ref zoom_state, zoom_out(size, speed));
    public void reset_zoom_state(float speed)               => state_swtich(ref zoom_state, reset_zoom(speed));
    public void move_up_state(float y_pos, float speed)     => state_swtich(ref swivel_state, move_up(y_pos, speed));
    public void move_down_state(float y_pos, float speed)   => state_swtich(ref swivel_state, move_down(y_pos, speed));
    public void reset_offset_state(float speed)             => state_swtich(ref swivel_state, reset_offset(speed));
    public void shake_camera(float time, float amount)      => state_swtich(ref follow_state, camera_shake(time, amount));

    // state switchers:
    void state_swtich(ref Coroutine state, IEnumerator n_state){
        if(state != null)
            StopCoroutine(state);
        state = StartCoroutine(n_state);
    }

    // states:
    IEnumerator follow(){
        while(true){
            Vector3 desired_pos = new Vector3(target.position.x + offset.x, offset.y, target.position.z + offset.z);
            // may want to swap this to Vector3.SmoothDamp();
            Vector3 smoothed_pos = Vector3.Lerp(transform.position, desired_pos, smooth_speed * Time.deltaTime);
            transform.position = regulate_position(smoothed_pos);            
            yield return new WaitForEndOfFrame();
        }
    }

    Vector3 regulate_position(in Vector3 pos){
        Vector3 regulated = new Vector3();
        regulated.x = pos.x < left_x_bound ? left_x_bound : (pos.x > right_x_bound ? right_x_bound : pos.x);
        regulated.y = pos.y < bot_y_bound ? bot_y_bound : (pos.y > top_y_bound ? top_y_bound : pos.y);
        regulated.z = pos.z;
        return regulated;

    }

    IEnumerator camera_shake(float time, float amount){
        float timer = 0;
        while(timer < time){
            float shake = (Random.Range(0,20) - 10) * amount;
            Vector3 desired_pos = new Vector3(target.position.x + offset.x + shake, offset.y + shake, target.position.z + offset.z);
            // may want to swap this to Vector3.SmoothDamp();
            Vector3 smoothed_pos = Vector3.Lerp(transform.position, desired_pos, smooth_speed * Time.deltaTime);
            transform.position = regulate_position(smoothed_pos);               
            timer += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        state_swtich(ref follow_state, follow());
        yield break;
    }

    IEnumerator move_up(float y_pos, float speed){
        while(offset.y < y_pos){
            offset += new Vector3(0, Time.deltaTime * speed, 0);
            yield return new WaitForEndOfFrame();
        }
        yield break;
    }

    IEnumerator move_down(float y_pos, float speed){
        while(offset.y > y_pos){
            offset -= new Vector3(0, Time.deltaTime * speed, 0);
            yield return new WaitForEndOfFrame();
        }
        yield break;
    }

    IEnumerator reset_offset(float speed){
        // determine if we are too high or too low.
        float difference = offset.y - original_offset.y;
        
        if(difference >= 0)
            state_swtich(ref swivel_state,move_down(original_offset.y, speed));
        else
            state_swtich(ref swivel_state,move_up(original_offset.y, speed));
        offset.y = original_offset.y;
        yield break;
    }

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

    IEnumerator test_zoom(){
        while(true){
            yield return new WaitForSeconds(3);
            if(zoom_state != null)
                StopCoroutine(zoom_state);
            zoom_state = StartCoroutine(zoom_out(18,3.75f));
            yield return new WaitForSeconds(3);
            if(zoom_state != null)
                StopCoroutine(zoom_state);
            zoom_state = StartCoroutine(zoom_in(10,3.75f));
            yield return new WaitForSeconds(3);
            if(zoom_state != null)
                StopCoroutine(zoom_state);
            zoom_state = StartCoroutine(reset_zoom(3.75f));
        }
    }

    public void snap_to_target(){
        transform.position = target.position + offset;
    }

    public void ZoomIn()    =>    cam.orthographicSize--;
    public void ZoomOut()   =>    cam.orthographicSize++;
}
