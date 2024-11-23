using UnityEngine;

public class CavalryCutsceneArrow : MonoBehaviour{
    [SerializeField] SmartTurret turret;
    AudioSource source;
    public void fire(){
        turret.fire_once();
        AudioClipHandler.play(
            SoundID.BOW_SHOT,
            audio_player:       this, 
            source:             out source, 
            randomise_pitch:    true, 
            spatial_blend:      true);
    }
}
