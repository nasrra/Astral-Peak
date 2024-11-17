using UnityEngine;

public class RiderParticlesHandler : ParticlesHandler{
    [SerializeField] ParticleSystem
        front_strike,
        signature_strike,
        jump_n_dash,
        jump_n_dash_strike,
        footstep,
        dash,
        jump,
        signature_slam,
        yell_particles;
    
    public void emit_signature_slam() => signature_slam.Play();
    public void emit_jump_effect() => jump.Play();
    public void play_dash_effect() => dash.Play();
    public void stop_dash_effect() => dash.Stop(true, stopBehavior: ParticleSystemStopBehavior.StopEmitting);
    public void emit_signature_strike() => signature_strike.Emit(1);
    public void emit_front_strike() => front_strike.Emit(1);
    public void emit_jump_n_dash() => jump_n_dash.Emit(1);
    public void emit_jump_n_dash_strike() => jump_n_dash_strike.Emit(1);
    public void emit_footstep() => footstep.Play();
    public void play_yell() => yell_particles.Play();
    public void stop_yell() => yell_particles.Stop(true, stopBehavior: ParticleSystemStopBehavior.StopEmitting);

    public override void flip_left(){
        flip_emitter_left(front_strike.GetComponent<ParticleSystemRenderer>());
        flip_emitter_left(signature_strike.GetComponent<ParticleSystemRenderer>());
        flip_emitter_left(jump_n_dash .GetComponent<ParticleSystemRenderer>());
        flip_emitter_left(jump_n_dash_strike .GetComponent<ParticleSystemRenderer>());
        flip_emitter_left(dash .GetComponent<ParticleSystemRenderer>());
    }

    public override void flip_right(){
        flip_emitter_right(front_strike.GetComponent<ParticleSystemRenderer>());
        flip_emitter_right(signature_strike.GetComponent<ParticleSystemRenderer>());
        flip_emitter_right(jump_n_dash .GetComponent<ParticleSystemRenderer>());
        flip_emitter_right(jump_n_dash_strike .GetComponent<ParticleSystemRenderer>());
        flip_emitter_right(dash .GetComponent<ParticleSystemRenderer>());
    }
}
