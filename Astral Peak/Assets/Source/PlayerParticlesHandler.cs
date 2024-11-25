using UnityEngine;

public class PlayerParticlesHandler : ParticlesHandler{
    string ground;
    [SerializeField] ParticleSystem 
        slash_effect, 
        dash_effect, 
        snow_footstep_effect,
        stone_footstep_effect, 
        snow_jump_effect,
        stone_jump_effect,
        death_effect,
        death_explosion;

    public void set_ground(string _ground) => ground = _ground;

    public override void flip_left(){
        flip_emitter_left(slash_effect.GetComponent<ParticleSystemRenderer>());
        flip_emitter_left(dash_effect.GetComponent<ParticleSystemRenderer>());
    }
    public override void flip_right(){
        flip_emitter_right(slash_effect.GetComponent<ParticleSystemRenderer>());
        flip_emitter_right(dash_effect.GetComponent<ParticleSystemRenderer>());
    }

    public void emit_jump()             => (ground == "Snow"? snow_jump_effect : stone_jump_effect).Play();
    public void emit_slash()            => slash_effect.Emit(1);
    public void emit_dash()             => dash_effect.Emit(1);
    public void emit_footstep()         => (ground == "Snow"? snow_footstep_effect : stone_footstep_effect).Play(); 
    public void play_death_particles()  => death_effect.Play();
    public void stop_death_particles()  => death_effect.Stop(true, ParticleSystemStopBehavior.StopEmitting);
    public void emit_death_expolosion() => death_explosion.Play();
}

