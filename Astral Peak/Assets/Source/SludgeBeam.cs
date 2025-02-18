using UnityEngine;

public class SludgeBeam : MonoBehaviour{
    [SerializeField] Collider2D hurtbox;
    [SerializeField] LineRenderer graphics;
    [SerializeField] ParticleSystem emitter_particle;
    [SerializeField] AudioPlayer audio_player;

    public void turn_on(){
        hurtbox.enabled = true;
        graphics.enabled = true;
        emitter_particle.Play();
        audio_player.play_diegetic_loop("water_rushing");
    }

    public void turn_off(){
        hurtbox.enabled = false;
        graphics.enabled = false;
        emitter_particle.Stop(false, ParticleSystemStopBehavior.StopEmitting);
        audio_player.stop_all_loops();
    }
}
