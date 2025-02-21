using System.Collections;
using Entropek;
using UnityEngine;

public class FinalBossRoomGroundHandler : MonoBehaviour{
    public Rigidbody2D[] grounds; // Array to hold all the squares
    public MeleeHolsterHandler holsters;
    [SerializeField] AudioPlayer audio_player;

    public void start_wave(int index, bool move_left, float rate, float force){
        int count = 0;
        if(index >= grounds.Length)
            return;
        if(move_left == true){
            for(int i = index; i >= 0; i--){
                int selected = i;
                use_ground(rate, force, count, selected);
                count++;
            }
        }
        else
            for(int i = index; i < grounds.Length; i++){
                int selected = i;
                use_ground(rate, force, count, selected);
                count++;
            }
    }

    public void use_ground(float rate, float force, int count, int selected){
        Rigidbody2D ground = grounds[selected];
        StartCoroutine(Util.timer(
            count * rate,
            time_out:()=>{
                ground.AddForce(new Vector2(0,force), ForceMode2D.Impulse);
                holsters.enable_melee_hurtbox($"{selected}");
                AudioManager.play_diegetic_one_shot("stone_shuffle",ground.gameObject);
            }
        ));
        StartCoroutine(Util.timer(
            count * rate + .5f,
            time_out:() =>      holsters.disable_melee_hurtbox($"{selected}")
        ));
    }
}

