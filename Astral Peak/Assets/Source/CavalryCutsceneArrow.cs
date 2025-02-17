using UnityEngine;

public class CavalryCutsceneArrow : MonoBehaviour{
    [SerializeField] AudioPlayer audio_player;
    [SerializeField] RangedHolster turret;
    public void fire(){
        turret.fire_once();
        audio_player.play_diegetic_one_shot("rider_bow_shot");
    }
}
