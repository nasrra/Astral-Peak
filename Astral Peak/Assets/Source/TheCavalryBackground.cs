using UnityEngine;

// this is a script for the background cavalry wolf that plays during the phase transition cutscene.

public class TheCavalryBackground : MonoBehaviour{
    [SerializeField] AudioPlayer audio_player;
    [SerializeField] ParticleSystem 
        impact_effect,
        hop_1,
        hop_2;
    public void emit_impact_effect() => impact_effect.Play();
    public void play_howl(){
        audio_player.play_non_diegetic_one_shot("wolf_howl");
    }
 
    public void play_hop(){
        audio_player.play_non_diegetic_one_shot("wolf_jump");  
    }
            
    public void emit_hop_1() => hop_1.Emit(1);
    public void emit_hop_2() => hop_2.Emit(1);
    public void move_camera_up() => CameraController.instance.lerp_offset(x:null, y:20, 2.25f);
    public void move_camera_down() => CameraController.instance.lerp_offset(x:null, y:-7, 0.25f);
    public void reset_camera() => CameraController.instance.reset_offset(.5f);
    public void play_impact_sound(){
        audio_player.play_non_diegetic_one_shot("snow_impact_heavy");  
    } 
    public void play_camera_shake() => CameraController.instance.shake_camera(time: 0.35f, amount: 2f, lock_shake: false);
}
