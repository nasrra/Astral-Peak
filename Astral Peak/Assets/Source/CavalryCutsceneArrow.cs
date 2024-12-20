using UnityEngine;
using Sounds;

public class CavalryCutsceneArrow : MonoBehaviour{
    [SerializeField] RangedHolster turret;
    public void fire(){
        turret.fire_once();
        AudioClipHandler.play(
            SoundID.BOW_SHOT,
            audio_player: this, 
            AudioSourceSettings.DIEGETIC_RANDOMISED);  
    }
}
