using System.Collections;
using UnityEngine;

public class FireController : SpriteHandler{
    [SerializeField] ParticleSystem embers, smoke;
    [SerializeField] AudioPlayer audio_player;

    public void off(){
        set_value("_Dim",10);
        enable_sprite(false);
    }

    public void on(){
        set_value("_Dim",1);
        enable_sprite(true);        
    }

    public void turn_off() {
        StartCoroutine(lerp_value(
            value: "_Dim",
            time: 3,
            start:1,
            end:10,
            callback:()=>{
                enable_sprite(false);
                smoke.Play();
                audio_player.play_diegetic_one_shot("fire_extinguish");
                audio_player.stop_all_loops();  
                embers.Emit(30);
                embers.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            }
        ));
    }

    public void turn_on(){
        audio_player.play_diegetic_loop("fire_crackle_soft");    
        embers.Emit(30);
        embers.Play();
        enable_sprite(true);
        StartCoroutine(lerp_value(
            value: "_Dim",
            time: 3,
            start:10,
            end:1
        ));        
    }
}
