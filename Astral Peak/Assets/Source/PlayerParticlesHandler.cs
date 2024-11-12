using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParticlesHandler : ParticlesHandler{
    [SerializeField] ParticleSystem slash_effect;
    [SerializeField] ParticleSystem dash_effect;
    
    public override void flip_left() => flip_emitter_left(slash_effect.GetComponent<ParticleSystemRenderer>());
    public override void flip_right() => flip_emitter_right(slash_effect.GetComponent<ParticleSystemRenderer>());

    public void emit_slash_effect() => slash_effect.Emit(1);
    public void emit_dash_effect() => dash_effect.Emit(1);
}

