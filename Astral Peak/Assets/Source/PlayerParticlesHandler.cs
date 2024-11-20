using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParticlesHandler : ParticlesHandler{
    [SerializeField] ParticleSystem slash_effect;
    [SerializeField] ParticleSystem dash_effect;
    [SerializeField] ParticleSystem footstep_effect;
    [SerializeField] ParticleSystem jump_effect;

    public override void flip_left(){
        flip_emitter_left(slash_effect.GetComponent<ParticleSystemRenderer>());
        flip_emitter_left(dash_effect.GetComponent<ParticleSystemRenderer>());
    }
    public override void flip_right(){
        flip_emitter_right(slash_effect.GetComponent<ParticleSystemRenderer>());
        flip_emitter_right(dash_effect.GetComponent<ParticleSystemRenderer>());
    }

    public void emit_jump()         => jump_effect.Play();
    public void emit_slash()        => slash_effect.Emit(1);
    public void emit_dash()         => dash_effect.Emit(1);
    public void emit_footstep()     => footstep_effect.Play(); 
}

