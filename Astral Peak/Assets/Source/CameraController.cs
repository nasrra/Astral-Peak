using UnityEngine;

public class CameraController : MonoBehaviour{
    
    public static CameraController instance;

    [SerializeField] private Transform target;
    [SerializeField, Range(0,5)] private float smooth_speed = 3.75f;
    [SerializeField] private Vector3 offset;
    [SerializeField] Camera cam;

    void Awake(){
        instance = this;
    }

    // Update is called once per frame
    void LateUpdate(){
        follow_target();
    }

    void follow_target(){
        Vector3 desired_pos = target.position + offset;
        // may want to swap this to Vector3.SmoothDamp();
        Vector3 smoothed_pos = Vector3.Lerp(transform.position, desired_pos, smooth_speed * Time.deltaTime);
        transform.position = smoothed_pos;
    }

    public void snap_to_target(){
        transform.position = target.position + offset;
    }

    public void ZoomIn()    =>     cam.orthographicSize--;
    public void ZoomOut()   =>    cam.orthographicSize++;
}
