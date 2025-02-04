using UnityEngine;

public class WaterfallHandler : MonoBehaviour{
    [SerializeField] LineRenderer line_renderer;
    [SerializeField] float length, speed;
    void Start(){
        line_renderer.positionCount = 2;
        line_renderer.SetPosition(0, transform.position);
        line_renderer.SetPosition(1, transform.position - Vector3.up * length);
    }
}
