using UnityEngine;

public class StoneDoorAniamtionHelper : MonoBehaviour{
    [SerializeField] AudioPlayer sound;
    public void invoke(){   
        sound.play_diegetic_one_shot("stone_door");
        CameraController.instance.shake_camera(3f, .15f, false);
    }
}
