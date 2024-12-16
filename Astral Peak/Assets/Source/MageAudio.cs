using System.Runtime.Serialization;
using Sounds;
using UnityEngine;

public class MageAudio : MonoBehaviour{
    public void emit_footstep_audio() => 
        AudioClipHandler.play(
            choose_footstep(),
            audio_player: this,
            AudioSourceSettings.DIEGETIC_RANDOMISED
        );

    public void emit_deep_croak_audio() =>
        AudioClipHandler.play(
            //SoundID.WOODEN_RATTLE_2,
            SoundID.NONE,
            audio_player: this,
            AudioSourceSettings.DIEGETIC_RANDOMISED
        );

    private SoundID choose_footstep(){
        int x = Random.Range(0,4);
        switch(x){
            case 0: return SoundID.SNOW_FOOTSTEP_1;
            case 1: return SoundID.SNOW_FOOTSTEP_2;
            case 2: return SoundID.SNOW_FOOTSTEP_3;
            case 3: return SoundID.SNOW_FOOTSTEP_4;
            default: return SoundID.NONE;
        }
    }

}
