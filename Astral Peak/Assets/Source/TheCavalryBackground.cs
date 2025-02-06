using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sounds;
using Unity.Collections;

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
            audio_player:       gameObject, 
            AudioSourceSettings.DIEGETIC_RANDOMISED);  
 
    public void play_hop() => 
        AudioClipHandler.play(
            SoundID.MAGIC_1,
            audio_player:       gameObject, 
            AudioSourceSettings.DIEGETIC_RANDOMISED);  
            
    public void emit_hop_1() => hop_1.Emit(1);
    public void emit_hop_2() => hop_2.Emit(1);
    public void move_camera_up() => CameraController.instance.lerp_offset(x:null, y:20, 2.25f);
    public void move_camera_down() => CameraController.instance.lerp_offset(x:null, y:-7, 0.25f);
    public void reset_camera() => CameraController.instance.reset_offset(.5f);
    public void play_impact_sound() => 
        AudioClipHandler.play(
            SoundID.SNOW_IMPACT_HEAVY,
            audio_player:       gameObject, 
            AudioSourceSettings.DIEGETIC_RANDOMISED);

    public void play_camera_shake() => CameraController.instance.shake_camera(time: 0.35f, amount: 2f, lock_shake: false);
}
