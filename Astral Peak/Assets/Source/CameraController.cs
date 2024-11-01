using System.Collections;
using UnityEditor.Rendering;
using UnityEngine;

public class CameraController : MonoBehaviour{
    
    public static CameraController instance;

    [SerializeField] private Transform target;
    [SerializeField, Range(0,5)] private float smooth_speed = 3.75f;
    [SerializeField] private Vector3 offset;
    [SerializeField] Camera cam;
    public float original_size;
    public Vector3 original_offset;

    Coroutine
        follow_state,
        zoom_state,
        test;

    void Awake(){
        instance = this;
        original_offset = offset;
        original_size = cam.orthographicSize;
        follow_state = StartCoroutine(follow());
        //test = StartCoroutine(test_zoom());
    }

    // external functions
    public void zoom_in_state(float size, float speed) => zoom_state_swtich(zoom_in(size, speed));
    public void zoom_out_state(float size, float speed) => zoom_state_swtich(zoom_out(size, speed));
    public void reset_zoom_state(float speed) => zoom_state_swtich(reset_zoom(speed));

    // states:
    IEnumerator follow(){
        while(true){
            Vector3 desired_pos = target.position + offset;
            // may want to swap this to Vector3.SmoothDamp();
            Vector3 smoothed_pos = Vector3.Lerp(transform.position, desired_pos, smooth_speed * Time.deltaTime);
            transform.position = smoothed_pos;            
            yield return new WaitForEndOfFrame();
        }
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
            zoom_state_swtich(zoom_in(original_size, speed));
        else
            zoom_state_swtich(zoom_out(original_size, speed));
        
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

    // state switchers:
    void zoom_state_swtich(IEnumerator state){
        if(zoom_state != null)
            StopCoroutine(zoom_state);
        zoom_state = StartCoroutine(state);
    }

    public void snap_to_target(){
        transform.position = target.position + offset;
    }

    public void ZoomIn()    =>    cam.orthographicSize--;
    public void ZoomOut()   =>    cam.orthographicSize++;
}
