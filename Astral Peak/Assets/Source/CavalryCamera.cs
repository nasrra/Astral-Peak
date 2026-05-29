using UnityEngine;

public class CavalryCamera : MonoBehaviour{
     public void ground_slam_camera_adjust(){
        CameraController.instance.lerp_offset(x:null,y:5f,time:1.5f);
        CameraController.instance.lerp_zoom(14, 1.5f);
        CameraController.instance.lerp_regulators(_x_bounds: new Vector2(-9.5f,9.5f), _y_bounds: null, time: 1.5f);
    }
    public void ground_slam_camera_reset(){
        CameraController.instance.reset_offset(.5f);
        CameraController.instance.reset_zoom(.5f);
        CameraController.instance.reset_regulators(.5f);
    }
    public void ground_slam_camera_shake()  => CameraController.instance.shake_camera(time:0.15f, amount:1f, lock_shake: false);
    public void death_camera_shake()        => CameraController.instance.shake_camera(time:0.25f, amount:1f, lock_shake: false);
}
