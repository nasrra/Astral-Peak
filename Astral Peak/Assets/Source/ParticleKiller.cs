using System.Collections.Generic;
using UnityEngine;

public class ParticleKiller : MonoBehaviour{
    [SerializeField] ParticleSystem particle_system;
    List<ParticleSystem.Particle> particles = new List<ParticleSystem.Particle>();
    private void OnParticleTrigger(){
        int triggered_particles = particle_system.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, particles);
        for(int i = 0; i < triggered_particles; i++){
            ParticleSystem.Particle p = particles[i];
            p.remainingLifetime = 0;
            particles[i] = p;
        }
        particle_system.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, particles);
    }
}
