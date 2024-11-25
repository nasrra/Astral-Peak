using UnityEditorInternal;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    AudioSource source;
    string ground;

    public void emit_attack()=> 
        AudioClipHandler.play(
            SoundID.MELEE_SWING_1,
            audio_player:       this, 
            source:             out source, 
            randomise_pitch:    true, 
            spatial_blend:      true);

    public void emit_dash()=> 
        AudioClipHandler.play(
            SoundID.WHOOSH_1,
            audio_player:       this, 
            source:             out source, 
            randomise_pitch:    true, 
            spatial_blend:      true);

    public void emit_footsteps()=> 
        AudioClipHandler.play(
            choose_footstep(),
            audio_player:       this, 
            source:             out source, 
            randomise_pitch:    true, 
            spatial_blend:      true);

    public void emit_grounded()=> 
        AudioClipHandler.play(
            choose_footstep(),
            audio_player:       this, 
            source:             out source, 
            randomise_pitch:    true, 
            spatial_blend:      true);

    public void emit_attack_hit()=> 
        AudioClipHandler.play(
            SoundID.MELEE_HIT,
            audio_player:       this, 
            source:             out source, 
            randomise_pitch:    true, 
            spatial_blend:      true);
    public void emit_jump()=> 
        AudioClipHandler.play(
            SoundID.WHOOSH_1,
            audio_player:       this, 
            source:             out source, 
            randomise_pitch:    true, 
            spatial_blend:      true);
    public void emit_damaged()=> 
        AudioClipHandler.play(
            SoundID.DEEP_BOOM,
            audio_player:       this, 
            source:             out source, 
            randomise_pitch:    false, 
            spatial_blend:      false);
            
    public void emit_magic_explosion() => 
        AudioClipHandler.play(
            SoundID.MAGIC_EXPLOSION,
            audio_player:       this, 
            source:             out source, 
            randomise_pitch:    true, 
            spatial_blend:      true);  

    public void set_ground(string _ground) => ground = _ground;

    public SoundID choose_footstep(){
        int x = Random.Range(0, 4);
        switch(ground){
            case "Snow":
                switch (x){
                    case 0: return SoundID.SNOW_FOOTSTEP_1;
                    case 1: return SoundID.SNOW_FOOTSTEP_2;
                    case 2: return SoundID.SNOW_FOOTSTEP_3;
                    case 3: return SoundID.SNOW_FOOTSTEP_4;
                }
            break;
            case "Stone":
                switch (x){
                    case 0: return SoundID.STONE_FOOTSTEP_1;
                    case 1: return SoundID.STONE_FOOTSTEP_2;
                    case 2: return SoundID.STONE_FOOTSTEP_3;
                    case 3: return SoundID.STONE_FOOTSTEP_4;
                }
            break;
        }
        throw new System.Exception("ERROR!");
    }
}

