using UnityEngine;
using AYellowpaper.SerializedCollections;

public class ParticlesHandler : MonoBehaviour{    
    [SerializedDictionary("id","particle system")]
    [SerializeField] protected SerializedDictionary<string, ParticleSystem> particles = new SerializedDictionary<string, ParticleSystem>();
    public virtual void emit(string particle_id, int amount) => particles[particle_id].Emit(amount);
    public virtual void play(string particle_id) => particles[particle_id].Play();
    public virtual void stop(string particle_id) => particles[particle_id].Stop(true, ParticleSystemStopBehavior.StopEmitting);

    public virtual void flip_left(){
        foreach(ParticleSystem p in particles.Values)
            flip_emitter_left(p.GetComponent<ParticleSystemRenderer>());
    }

    public virtual void flip_right(){
        foreach(ParticleSystem p in particles.Values)
            flip_emitter_right(p.GetComponent<ParticleSystemRenderer>());
    }

    public void stop_all_particles(){
        foreach(string p in particles.Keys)
            stop(p);
    }
    public void flip_emitter_left(ParticleSystemRenderer emitter) => emitter.flip = new Vector3(1, emitter.flip.y, 0);
    public void flip_emitter_right(ParticleSystemRenderer emitter) => emitter.flip = new Vector3(0, emitter.flip.y, 0);
}


