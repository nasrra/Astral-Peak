using AYellowpaper.SerializedCollections;
using UnityEngine;

public class RiderParticlesHandler : ParticlesHandler{
    [SerializedDictionary("id","particle system")]
    public override void emit(string particle_id, int amount) => particles[particle_id].Emit(amount);
    public override void play(string particle_id) => particles[particle_id].Play();
    public override void stop(string particle_id) => particles[particle_id].Stop(true, ParticleSystemStopBehavior.StopEmitting);

    public override void flip_left(){
        foreach(ParticleSystem p in particles.Values)
            flip_emitter_left(p.GetComponent<ParticleSystemRenderer>());
    }

    public override void flip_right(){
        foreach(ParticleSystem p in particles.Values)
            flip_emitter_right(p.GetComponent<ParticleSystemRenderer>());
    }
}
