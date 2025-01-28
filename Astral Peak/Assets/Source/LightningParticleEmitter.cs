using UnityEngine;

public class LightningParticleEmitter : LineParticleEmitter{
    AudioSource source;
    
    protected override void emitted(){
        int x = Random.Range(0,2);
        if(x==0)
            AudioClipHandler.play(
                sound_id: Sounds.SoundID.THUNDER_1,
                audio_player: this,
                AudioSourceSettings.DIEGETIC_RANDOMISED
            );
        else  
            AudioClipHandler.play(
                sound_id: Sounds.SoundID.THUNDER_2,
                audio_player: this,
                AudioSourceSettings.DIEGETIC_RANDOMISED
            );      
        CameraController.instance.shake_camera(.33f, .75f, false);
        SceneLighting.instance.set_intensity("global", 3f);
        SceneLighting.instance.reset_lighting("global", .2f);
    }

    protected override void emitting(){
        source = AudioClipHandler.play(
            sound_id: Sounds.SoundID.ELECTRICITY_LOOP,
            audio_player: this,
            AudioSourceSettings.DIEGETIC_RANDOMISED_LOOP
        ); 
    }

    protected override void ended(){
        if(source !=null)
            StartCoroutine(AudioClipHandler.fade_out(source, 2, destroy_source: true));
    }
}
