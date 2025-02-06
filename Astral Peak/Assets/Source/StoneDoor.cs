using UnityEngine;

public class StoneDoorAniamtionHelper : MonoBehaviour{
    public void invoke(){
        AudioClipHandler.play(Sounds.SoundID.STONE_DOOR, gameObject, AudioSourceSettings.DIEGETIC_RANDOMISED);
        CameraController.instance.shake_camera(3f, .15f, false);
    }
}
