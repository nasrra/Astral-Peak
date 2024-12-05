using UnityEngine;

public class HollowAudio : MonoBehaviour{
    public void yell_audio() =>
        AudioClipHandler.play(
            SoundID.RIDER_YELL,
            audio_player: this, 
            AudioSourceSettings.DIEGETIC);  

    public void footstep_audio() =>
        AudioClipHandler.play(
            choose_footstep(),
            audio_player: this, 
            AudioSourceSettings.DIEGETIC);  

    SoundID choose_footstep(){
        int x = Random.Range(0, 4);
        switch (x){
            case 0: return SoundID.STONE_FOOTSTEP_1;
            case 1: return SoundID.STONE_FOOTSTEP_2;
            case 2: return SoundID.STONE_FOOTSTEP_3;
            case 3: return SoundID.STONE_FOOTSTEP_4;
        }
        return SoundID.NONE;
    }
}
