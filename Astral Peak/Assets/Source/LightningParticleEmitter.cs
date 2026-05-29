using UnityEngine;

public class LightningParticleEmitter : LineParticleEmitter{
    [SerializeField] AudioPlayer audio_player;
    [SerializeField] ParticleSystem end_point_particles;
    bool is_emitting = false;

    protected override void emitted(){
        int x = Random.Range(0,2);
        audio_player.play_diegetic_one_shot("thunder_clap");
        CameraController.instance.shake_camera(.33f, .75f, false);
        SceneLighting.instance.set_intensity("global", 3f);
        SceneLighting.instance.reset_lighting("global", .2f);
        end_point_particles.Emit(10);
    }

    protected override void emitting(){
        audio_player.play_diegetic_loop("electricity_crackle_harsh");
        is_emitting = true;
        end_point_particles.Play();
    }

    protected override void ended(){
        if(is_emitting == true){
            audio_player.stop_all_loops();
            end_point_particles.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            is_emitting = false;
        }
    }
}
