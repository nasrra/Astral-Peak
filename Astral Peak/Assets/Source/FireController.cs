using System.Collections;
using UnityEngine;
using Sounds;

public class FireController : SpriteHandler{
    [SerializeField] ParticleSystem embers, smoke;
    AudioSource fire_source;

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
                AudioClipHandler.play(
                    SoundID.STEAM,
                    audio_player: gameObject, 
                    AudioSourceSettings.DIEGETIC);  
                fire_source.Stop();
                embers.Emit(30);
                embers.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                Destroy(fire_source);
            }
        ));
    }

    public void turn_on(){
        fire_source = AudioClipHandler.play(
            SoundID.SMALL_FIRE, 
            audio_player: gameObject,
            AudioSourceSettings.DIEGETIC_LOOP);  
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
