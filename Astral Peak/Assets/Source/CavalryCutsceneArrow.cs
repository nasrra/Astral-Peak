using UnityEngine;

public class CavalryCutsceneArrow : MonoBehaviour{
    [SerializeField] RangedHolster turret;
    public void fire(){
        turret.fire_once();
        // AudioClipHandler.play(
        //     SoundID.BOW_SHOT,
        //     audio_player: gameObject, 
        //     AudioSourceSettings.DIEGETIC_RANDOMISED);  
    }
}
