using UnityEngine;

public class Trigger2DCameraLink : MonoBehaviour{
    [SerializeField] Vector3 offset;
    void OnTriggerEnter2D() => CameraController.instance.set_offset(offset, true);
}
