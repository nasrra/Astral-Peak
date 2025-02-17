using UnityEngine;

public class LightningParticleEmitter : LineParticleEmitter{
    [SerializeField] AudioPlayer audio_player;
    bool is_emitting = false;

    protected override void emitted(){
        int x = Random.Range(0,2);
        audio_player.play_diegetic_one_shot("thunder_clap");
        CameraController.instance.shake_camera(.33f, .75f, false);
        SceneLighting.instance.set_intensity("global", 3f);
        SceneLighting.instance.reset_lighting("global", .2f);
    }

    protected override void emitting(){
        audio_player.play_diegetic_loop("electricity_crackle_harsh");
        is_emitting = true;
    }

    protected override void ended(){
        if(is_emitting == true){
            audio_player.stop_loop("electricity_crackle_harsh");
            is_emitting = false;
        }
    }
}
