using UnityEngine;

public class StoneDoorAniamtionHelper : MonoBehaviour{
    [SerializeField] ParticleHandler particles;
    [SerializeField] AudioPlayer audio_player;
    public void started_transitioning(){
        particles.play_particle("transition_ambience");
        audio_player.play_diegetic_one_shot("stone_door");
        CameraController.instance.shake_camera(3f, .15f, false);
    }
    public void finished_transitioning(){
        particles.stop_all_particles();
        particles.get_particle("transition_ambience").Emit(15);
        audio_player.play_diegetic_one_shot("stone_door_shut");
    }

}
