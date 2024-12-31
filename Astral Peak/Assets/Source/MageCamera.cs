using UnityEngine;

public class MageCamera : MonoBehaviour{
    [SerializeField] LightningController lightning;
    public void signature_adjust(){
        CameraController.instance.lerp_offset(x:null, y:6f, time:2);
        CameraController.instance.lerp_zoom(size: 16, time: 2);
        CameraController.instance.lerp_regulators(_x_bounds: Vector2.zero, _y_bounds: null, time: 2);
    }
    public void signature_shake() => CameraController.instance.shake_camera(amount: .5f, time: 7.30f);
    public void signature_reset(){
        CameraController.instance.reset_offset(2);
        CameraController.instance.reset_zoom(2);
        CameraController.instance.reset_regulators(2);
    }
}
