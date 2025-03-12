using AYellowpaper.SerializedCollections;
using UnityEngine;

public class ParticleHandler : MonoBehaviour{
    [SerializedDictionary("id","particle system")]
    [SerializeField] SerializedDictionary<string, ParticleSystem> particles = new SerializedDictionary<string, ParticleSystem>();
    public void emit_particle(string particle_id)                   => particles[particle_id].Emit(1);
    public void play_particle(string particle_id)                   => particles[particle_id].Play();
    public void stop_particle(string particle_id)                   => particles[particle_id].Stop(true, ParticleSystemStopBehavior.StopEmitting);
    private void flip_emitter_left(ParticleSystemRenderer emitter) => emitter.flip = new Vector3(1, emitter.flip.y, 0);
    private void flip_emitter_right(ParticleSystemRenderer emitter) => emitter.flip = new Vector3(0, emitter.flip.y, 0);
    public void stop_all_particles(){
        foreach(ParticleSystem p in particles.Values)
            p.Stop(false,ParticleSystemStopBehavior.StopEmitting);
    }
    public void flip_particles_right(){
        if(particles.Count <= 0)
            return;
        foreach(ParticleSystem p in particles.Values)
            flip_emitter_right(p.GetComponent<ParticleSystemRenderer>());
    }
    public void flip_particles_left(){
        if(particles.Count <= 0)
            return;
        foreach(ParticleSystem p in particles.Values)
            flip_emitter_left(p.GetComponent<ParticleSystemRenderer>());
    }
    public ParticleSystem get_particle(string id) => particles[id];
}


