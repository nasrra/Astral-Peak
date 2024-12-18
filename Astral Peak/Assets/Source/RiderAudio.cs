using UnityEngine;
using Sounds;

public class RiderAudio : MonoBehaviour{
    public void whistle() => 
        AudioClipHandler.play(
            SoundID.WHISTLE_LONG,
            audio_player: this, 
            AudioSourceSettings.DIEGETIC);  

    public void arrow_shot()=> 
        AudioClipHandler.play(
            SoundID.BOW_SHOT,
            audio_player:       this, 
            AudioSourceSettings.DIEGETIC_RANDOMISED);  

    public void magic()=> 
        AudioClipHandler.play(
            SoundID.MAGIC_1,
            audio_player:       this, 
            AudioSourceSettings.DIEGETIC_RANDOMISED);  

    public void melee_attack()=> 
        AudioClipHandler.play(
            SoundID.MELEE_SWING_2,
            audio_player:       this, 
            AudioSourceSettings.DIEGETIC_RANDOMISED);  

    public void yell()=> 
        AudioClipHandler.play(
            SoundID.RIDER_YELL,
            audio_player:       this, 
            AudioSourceSettings.DIEGETIC_RANDOMISED);  

    public void footstep() =>
        AudioClipHandler.play(
            choose_footstep(),
            audio_player: this,
            AudioSourceSettings.DIEGETIC_RANDOMISED);
    
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

