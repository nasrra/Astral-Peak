using UnityEngine;

public class MageParticles : ParticlesHandler{
    [SerializeField] ParticleSystem footsteps;

    public void emit_footstep_particle() => footsteps.Play();

    public override void flip_left(){
        throw new System.NotImplementedException();
    }

    public override void flip_right(){
        throw new System.NotImplementedException();
    }
}
