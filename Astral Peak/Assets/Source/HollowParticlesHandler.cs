using UnityEngine;

public class HollowParticlesHandler : MonoBehaviour{
    [SerializeField] ParticleSystem yell_particles;
    public void play_yell() => yell_particles.Play();
    public void stop_yell() => yell_particles.Stop(true, ParticleSystemStopBehavior.StopEmitting);
}
