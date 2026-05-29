using UnityEngine;

public class MageCamera : MonoBehaviour{
    public void yell_camera_shake() => CameraController.instance.shake_camera(time: 3, amount: 0.75f, lock_shake: false);
}