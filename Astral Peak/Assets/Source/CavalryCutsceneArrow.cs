using UnityEngine;

public class CavalryCutsceneArrow : MonoBehaviour{
    [SerializeField] SmartTurret turret;
    AudioSource source;
    public void fire(){
        turret.fire_once();
        AudioClipHandler.play(this,SoundID.BOW_SHOT,out source);
    }
}
