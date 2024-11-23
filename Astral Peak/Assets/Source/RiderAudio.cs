using UnityEngine;

public class RiderAudio : MonoBehaviour{
    AudioSource source;

    public void emit_whistle() => 
        AudioClipHandler.play(
            SoundID.WHISTLE_LONG,
            audio_player:       this, 
            source:             out source, 
            randomise_pitch:    true, 
            spatial_blend:      true);
    public void emit_arrow_shot()=> 
        AudioClipHandler.play(
            SoundID.BOW_SHOT,
            audio_player:       this, 
            source:             out source, 
            randomise_pitch:    true, 
            spatial_blend:      true);
    public void emit_magic()=> 
        AudioClipHandler.play(
            SoundID.MAGIC_1,
            audio_player:       this, 
            source:             out source, 
            randomise_pitch:    true, 
            spatial_blend:      true);
    public void emit_melee_attack()=> 
        AudioClipHandler.play(
            SoundID.MELEE_SWING_2,
            audio_player:       this, 
            source:             out source, 
            randomise_pitch:    true, 
            spatial_blend:      true);
    public void emit_yell()=> 
        AudioClipHandler.play(
            SoundID.RIDER_YELL,
            audio_player:       this, 
            source:             out source, 
            randomise_pitch:    true, 
            spatial_blend:      true);
}

