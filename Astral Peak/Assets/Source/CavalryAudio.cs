using UnityEngine;
using Sounds;

public class CavalryAudio : MonoBehaviour
{
    AudioSource source;

    public void emit_arrow_knocked()=> 
        AudioClipHandler.play(
            SoundID.COIN_TOSS,
            audio_player: this, 
            AudioSourceSettings.DIEGETIC);

    public void emit_bark()=> 
        AudioClipHandler.play(
            SoundID.DOG_BARK_1,
            audio_player: this, 
            AudioSourceSettings.DIEGETIC_RANDOMISED);  

    public void emit_bow_shot()=> 
        AudioClipHandler.play(
            SoundID.BOW_SHOT,
            audio_player: this, 
            AudioSourceSettings.DIEGETIC_RANDOMISED);

    public void emit_front_strike_grab()=> 
        AudioClipHandler.play(
            SoundID.LEATHER_CONTORT_1,
            audio_player: this, 
            AudioSourceSettings.DIEGETIC);  

    public void emit_ground_slam_hop()=> 
        AudioClipHandler.play(
            SoundID.MAGIC_1,
            audio_player: this, 
            AudioSourceSettings.DIEGETIC);  

    public void emit_ground_slam_impact() => 
        AudioClipHandler.play(
            SoundID.SNOW_IMPACT_HEAVY,
            audio_player: this, 
            AudioSourceSettings.DIEGETIC);  

    public void emit_howl()=> 
        AudioClipHandler.play(
            SoundID.WOLF_HOWL,
            audio_player: this, 
            AudioSourceSettings.DIEGETIC);  

    public void emit_sword_strike() => 
        AudioClipHandler.play(
            SoundID.MELEE_SWING_3,
            audio_player: this, 
            AudioSourceSettings.DIEGETIC);  

    public void emit_footsteps() => 
        AudioClipHandler.play(
            choose_footstep(),
            audio_player: this, 
            AudioSourceSettings.DIEGETIC_RANDOMISED);  

    public void emit_magic_explosion() => 
        AudioClipHandler.play(
            SoundID.MAGIC_EXPLOSION,
            audio_player: this, 
            AudioSourceSettings.DIEGETIC);  

    public SoundID choose_footstep()
    {
        int x = Random.Range(0, 4);
        switch (x)
        {
            case 0: return SoundID.SNOW_FOOTSTEP_1;
            case 1: return SoundID.SNOW_FOOTSTEP_2;
            case 2: return SoundID.SNOW_FOOTSTEP_3;
            case 3: return SoundID.SNOW_FOOTSTEP_4;
        }
        throw new System.Exception("ERROR!");
    }
}
