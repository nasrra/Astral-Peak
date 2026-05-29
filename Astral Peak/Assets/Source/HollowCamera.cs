using UnityEngine;

public class HollowCamera : MonoBehaviour{
    public void alert_camera_shake() => CameraController.instance.shake_camera(time: 2,    amount: .55f, lock_shake: false);
    public void death_camera_shake() => CameraController.instance.shake_camera(time: .55f, amount: .70f, lock_shake: false);
}
