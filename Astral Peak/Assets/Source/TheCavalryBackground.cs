using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this is a script for the background cavalry wolf that plays during the phase transition cutscene.

public class TheCavalryBackground : MonoBehaviour{
    AudioSource source;
    [SerializeField] ParticleSystem 
        impact_effect,
        hop_1,
        hop_2;
    public void emit_impact_effect() => impact_effect.Play();
    public void play_howl() => 
        AudioClipHandler.play(
            SoundID.WOLF_HOWL,
            audio_player:       this, 
            AudioSourceSettings.DIEGETIC_RANDOMISED);  
 
    public void play_hop() => 
        AudioClipHandler.play(
            SoundID.MAGIC_1,
            audio_player:       this, 
            AudioSourceSettings.DIEGETIC_RANDOMISED);  
            
    public void emit_hop_1() => hop_1.Emit(1);
    public void emit_hop_2() => hop_2.Emit(1);
    public void move_camera_up() => CameraController.instance.move_vertical_state(22, 7f);
    public void reset_camera() => CameraController.instance.reset_offset_state(1.5f);
    public void play_impact_sound() => 
        AudioClipHandler.play(
            SoundID.SNOW_IMPACT_HEAVY,
            audio_player:       this, 
            AudioSourceSettings.DIEGETIC_RANDOMISED);

    public void play_camera_shake() => CameraController.instance.shake_camera(0.35f,1f);
}
