using JetBrains.Annotations;
using UnityEngine;

public class CavalryParticlesHandler : ParticlesHandler{
    [SerializeField] ParticleSystem
        front_strike            ,
        back_strike_backward    ,
        back_strike_forward     ,
        top_bite                ,
        bottom_bite             ,
        ground_slam_hop_first   ,
        ground_slam_hop_second  ,
        ground_slam_impact      ,
        front_footstep          ,
        back_footstep           ,
        jump_particle           ,
        dash_effect             ,
        death_explosion         ,
        death_particles;
    
    public void emit_front_strike        ()     => front_strike.Emit(1);
    public void emit_back_strike_backward()     => back_strike_backward.Emit(1);
    public void emit_back_strike_forward ()     => back_strike_forward.Emit(1);
    public void emit_ground_slam_hop_first()    => ground_slam_hop_first.Emit(1);
    public void emit_ground_slam_hop_second()   => ground_slam_hop_second.Emit(1);
    public void emit_ground_slam_impact()       => ground_slam_impact.Play();
    public void play_death_particles()          => death_particles.Play();
    public void stop_death_particles()          => death_particles.Stop(true, ParticleSystemStopBehavior.StopEmitting);
    public void emit_front_footstep()           => front_footstep.Play();
    public void emit_back_footstep()            => back_footstep.Play();
    public void emit_jump_particle()            => jump_particle.Play();
    public void play_dash_effect()              => dash_effect.Play();
    public void stop_dash_effect()              => dash_effect.Stop(true, ParticleSystemStopBehavior.StopEmitting);
    public void emit_death_explosion()          => death_explosion.Play();
    public void emit_bite(){
        top_bite.Emit(1);
        bottom_bite.Emit(1);
    }

    public override void flip_left(){
        flip_emitter_left(front_strike          .GetComponent<ParticleSystemRenderer>());
        flip_emitter_left(back_strike_backward  .GetComponent<ParticleSystemRenderer>());
        flip_emitter_left(back_strike_forward   .GetComponent<ParticleSystemRenderer>());
        flip_emitter_left(top_bite              .GetComponent<ParticleSystemRenderer>());
        flip_emitter_left(bottom_bite           .GetComponent<ParticleSystemRenderer>());
        flip_emitter_left(ground_slam_hop_first .GetComponent<ParticleSystemRenderer>());
        flip_emitter_left(ground_slam_hop_second.GetComponent<ParticleSystemRenderer>());
        flip_emitter_left(ground_slam_impact    .GetComponent<ParticleSystemRenderer>());
        flip_emitter_left(dash_effect           .GetComponent<ParticleSystemRenderer>());
    }

    public override void flip_right(){
        flip_emitter_right(front_strike          .GetComponent<ParticleSystemRenderer>());
        flip_emitter_right(back_strike_backward  .GetComponent<ParticleSystemRenderer>());
        flip_emitter_right(back_strike_forward   .GetComponent<ParticleSystemRenderer>());
        flip_emitter_right(top_bite              .GetComponent<ParticleSystemRenderer>());
        flip_emitter_right(bottom_bite           .GetComponent<ParticleSystemRenderer>());
        flip_emitter_right(ground_slam_hop_first .GetComponent<ParticleSystemRenderer>());
        flip_emitter_right(ground_slam_hop_second.GetComponent<ParticleSystemRenderer>()); 
        flip_emitter_right(ground_slam_impact    .GetComponent<ParticleSystemRenderer>());
        flip_emitter_right(dash_effect           .GetComponent<ParticleSystemRenderer>());
    }

    public void stop_all_particles(){
        front_strike          .Stop(true, ParticleSystemStopBehavior.StopEmitting);
        back_strike_backward  .Stop(true, ParticleSystemStopBehavior.StopEmitting);
        back_strike_forward   .Stop(true, ParticleSystemStopBehavior.StopEmitting);
        top_bite              .Stop(true, ParticleSystemStopBehavior.StopEmitting);
        bottom_bite           .Stop(true, ParticleSystemStopBehavior.StopEmitting);
        ground_slam_hop_first .Stop(true, ParticleSystemStopBehavior.StopEmitting);
        ground_slam_hop_second.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        ground_slam_impact    .Stop(true, ParticleSystemStopBehavior.StopEmitting);
        front_footstep        .Stop(true, ParticleSystemStopBehavior.StopEmitting);
        back_footstep         .Stop(true, ParticleSystemStopBehavior.StopEmitting);
        jump_particle         .Stop(true, ParticleSystemStopBehavior.StopEmitting);
        dash_effect           .Stop(true, ParticleSystemStopBehavior.StopEmitting);
        death_explosion       .Stop(true, ParticleSystemStopBehavior.StopEmitting);
        death_particles       .Stop(true, ParticleSystemStopBehavior.StopEmitting);     
    }
}
