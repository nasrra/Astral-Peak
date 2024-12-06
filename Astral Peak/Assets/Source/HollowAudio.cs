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
    
    public void walk_rattle_audio() =>
        AudioClipHandler.play(
            SoundID.WOODEN_RATTLE_1,
            audio_player: this,
            AudioSourceSettings.DIEGETIC_RANDOMISED);

    public void idle_rattle_audio() =>
        AudioClipHandler.play(
            SoundID.WOODEN_RATTLE_2,
            audio_player: this,
            AudioSourceSettings.DIEGETIC_RANDOMISED);
    
    public void death_explosion_audio() =>
        AudioClipHandler.play(
            SoundID.MAGIC_EXPLOSION,
            audio_player: this,
            AudioSourceSettings.DIEGETIC_RANDOMISED);

    public void death_rattle_audio() =>
        AudioClipHandler.play(
            SoundID.WOODEN_RATTLE_4,
            audio_player: this,
            AudioSourceSettings.DIEGETIC_RANDOMISED);
    public void play_alerted_music() =>
        AudioManager.play_music(SoundID.HOLLOW_THEME);

    public void stop_alerted_music() =>
        AudioManager.stop_music();


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
