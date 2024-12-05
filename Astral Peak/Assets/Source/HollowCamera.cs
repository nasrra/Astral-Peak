using UnityEngine;

public class HollowCamera : MonoBehaviour{
    public void alert_camera_shake() => CameraController.instance.shake_camera(2, .55f);
}
