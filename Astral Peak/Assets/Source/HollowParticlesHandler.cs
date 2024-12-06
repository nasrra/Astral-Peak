using UnityEngine;

public class HollowParticlesHandler : MonoBehaviour{
    [SerializeField] ParticleSystem 
        ambience, yell, death_explosion, death_ambience;
    public void play_yell_particles() => yell.Play();
    public void stop_yell_particles() => yell.Stop(true, ParticleSystemStopBehavior.StopEmitting);
    public void play_ambient_particles() => ambience.Play();
    public void stop_ambient_particles() => ambience.Stop(true, ParticleSystemStopBehavior.StopEmitting);
    public void emit_death_explosion_particles() => death_explosion.Play();
    public void play_death_ambient_particles() => death_ambience.Play();
    public void stop_death_ambient_particles() => death_ambience.Stop(true, ParticleSystemStopBehavior.StopEmitting);
}
