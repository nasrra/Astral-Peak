using UnityEngine;

public class RiderParticlesHandler : ParticlesHandler{
    [SerializeField] ParticleSystem
        front_strike;
    
    public void emit_front_strike() => front_strike.Emit(1);
    public override void flip_left(){
        front_strike    .GetComponent<ParticleSystemRenderer>().flip = left;
    }

    public override void flip_right(){
        front_strike    .GetComponent<ParticleSystemRenderer>().flip = right;
    }
}
