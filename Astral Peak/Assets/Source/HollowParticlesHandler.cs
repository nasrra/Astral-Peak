using UnityEngine;

public class HollowParticlesHandler : MonoBehaviour{
    [SerializeField] ParticleSystem 
        ambience, yell, death_explosion, death_ambience;
    public void play_yell() => yell.Play();
    public void stop_yell() => yell.Stop(true, ParticleSystemStopBehavior.StopEmitting);
    public void play_ambience() => ambience.Play();
    public void stop_ambience() => ambience.Stop(true, ParticleSystemStopBehavior.StopEmitting);
    public void emit_death_explosion() => death_explosion.Play();
    public void play_death_ambience() => death_ambience.Play();
    public void stop_death_ambience() => death_ambience.Stop(true, ParticleSystemStopBehavior.StopEmitting);
}
