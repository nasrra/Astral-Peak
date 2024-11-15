using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour{
    Sound 
        dash,
        attack;
    Sound[]
        snow_footstep = new Sound[4];
    
    AudioSource
        source;

    void Start(){
        dash                = SoundLibrary.sfx["whoosh_1"]();
        snow_footstep[0]    = SoundLibrary.sfx["snow_footstep_1"]();
        snow_footstep[1]    = SoundLibrary.sfx["snow_footstep_2"]();
        snow_footstep[2]    = SoundLibrary.sfx["snow_footstep_3"]();
        snow_footstep[3]    = SoundLibrary.sfx["snow_footstep_4"]();  
        attack              = SoundLibrary.sfx["melee_swing_1"]();     
    }

    public void emit_dash()      => AudioClipHandler.play(this, dash, out source);                                          
    public void emit_attack()    => AudioClipHandler.play(this, attack, out source);                            
    public void emit_footsteps() => AudioClipHandler.play(this, snow_footstep[Random.Range(0,4)],out source);   
}
