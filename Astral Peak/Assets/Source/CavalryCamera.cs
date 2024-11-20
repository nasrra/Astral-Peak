using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CavalryCamera : MonoBehaviour{
     public void ground_slam_camera_adjust(){
        CameraController.instance.move_up_state(6, 4f);
        CameraController.instance.zoom_out_state(12, 2f);
    }
    public void ground_slam_camera_reset(){
        CameraController.instance.reset_offset_state(32f);
        CameraController.instance.reset_zoom_state(16f);
    }
    public void sword_summon_camera_zoom()  => CameraController.instance.zoom_out_state(14, 2f);
    public void sword_summon_camera_reset() => CameraController.instance.reset_zoom_state(1f);
    public void ground_slam_camera_shake()  => CameraController.instance.shake_camera(0.15f, 0.65f);
    public void death_camera_shake()        => CameraController.instance.shake_camera(0.25f, 1f);
}
