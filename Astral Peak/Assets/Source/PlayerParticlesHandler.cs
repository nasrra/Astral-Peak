using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParticlesHandler : ParticlesHandler{
    [SerializeField] ParticleSystem slash_effect;
    
    public override void flip_left(){
        slash_effect.GetComponent<ParticleSystemRenderer>().flip = left;
    }

    public override void flip_right(){
        slash_effect.GetComponent<ParticleSystemRenderer>().flip = right;
    }

    public void emit_slash_effect() => slash_effect.Emit(1);
}
