using System.Collections;
using Entropek;
using UnityEngine;

public class FinalBossRoomGroundHandler : MonoBehaviour{
    public Rigidbody2D[] grounds; // Array to hold all the squares
    public SludgeGeyser[] geysers;
    public MeleeHolsterHandler holsters;
    [SerializeField] AudioPlayer audio_player;
    Coroutine geysers_state;

    void Awake(){
        //StartCoroutine(test());
    }

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
                audio_player.play_diegetic_one_shot_gameobject("stone_shuffle",ground.gameObject);
            }
        ));
        StartCoroutine(Util.timer(
            count * rate + .5f,
            time_out:() =>      holsters.disable_melee_hurtbox($"{selected}")
        ));
    }

    IEnumerator test(){
        while(true){
            use_geysers();
            yield return new WaitForSeconds(6);
            use_geysers();
            yield return new WaitForSeconds(6);
            yield return null;
        }
    }

    public void use_geysers(){
        if(geysers_state != null)
            return;
        geysers_state = StartCoroutine(beams_coroutine());
    }

    IEnumerator beams_coroutine(){
        for(int i = 0; i < geysers.Length; i++){
            geysers[i].turn_on();
            geysers[i].lerp_length(1,.25f);
        }
        CameraController.instance.shake_camera(1f, 0.45f, true);
        yield return new WaitForSeconds(1f);
        for(int i = 0; i < geysers.Length; i++){
            geysers[i].lerp_length(15,.25f);
            geysers[i].set_sound_intensity(1);
            geysers[i].play_burst_sound();
        }
        CameraController.instance.shake_camera(2f, 0.85f, true);
        yield return new WaitForSeconds(2);
        for(int i = 0; i < geysers.Length; i++){
            geysers[i].lerp_length(0,.25f);
            geysers[i].set_sound_intensity(0);
        }
        yield return new WaitForSeconds(.25f);
        for(int i = 0; i < geysers.Length; i++){
            geysers[i].turn_off();
        }     
        geysers_state = null;  
    }
}

