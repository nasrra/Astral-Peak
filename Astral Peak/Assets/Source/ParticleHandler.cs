using AYellowpaper.SerializedCollections;
using UnityEngine;

public class ParticleHandler : MonoBehaviour{
    string ground;
    [SerializedDictionary("id","particle system")]
    [SerializeField] SerializedDictionary<string, ParticleSystem> particles = new SerializedDictionary<string, ParticleSystem>();
    public void emit_particle(string particle_id)                   => particles[particle_id].Emit(1);
    public void play_particle(string particle_id)                   => particles[particle_id].Play();
    public void stop_particle(string particle_id)                   => particles[particle_id].Stop(true, ParticleSystemStopBehavior.StopEmitting);
    public void play_ground_effected_particle(string particle_id){
        if(ground == "")
            return;
        particles[ground+"_"+particle_id].Play();
    }
    private void flip_emitter_left(ParticleSystemRenderer emitter) => emitter.flip = new Vector3(1, emitter.flip.y, 0);
    private void flip_emitter_right(ParticleSystemRenderer emitter) => emitter.flip = new Vector3(0, emitter.flip.y, 0);
    public void set_ground(string _ground) => ground = _ground;
    public void stop_all_particles(){
        foreach(string p in particles.Keys)
            stop_particle(p);
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
}


