using UnityEngine;

public class CavalryParticlesHandler : ParticlesHandler{
    [SerializeField] ParticleSystem
        front_strike            ,
        back_strike_backward    ,
        back_strike_forward     ,
        top_bite                ,
        bottom_bite             ,
        ground_slam_hop_first ,
        ground_slam_hop_second;
    
    public void emit_front_strike        () => front_strike.Emit(1);
    public void emit_back_strike_backward() => back_strike_backward.Emit(1);
    public void emit_back_strike_forward () => back_strike_forward.Emit(1);
    public void emit_ground_slam_hop_first() => ground_slam_hop_first.Emit(1);
    public void emit_ground_slam_hop_second() => ground_slam_hop_second.Emit(1);
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
    }

    public override void flip_right(){
        flip_emitter_right(front_strike          .GetComponent<ParticleSystemRenderer>());
        flip_emitter_right(back_strike_backward  .GetComponent<ParticleSystemRenderer>());
        flip_emitter_right(back_strike_forward   .GetComponent<ParticleSystemRenderer>());
        flip_emitter_right(top_bite              .GetComponent<ParticleSystemRenderer>());
        flip_emitter_right(bottom_bite           .GetComponent<ParticleSystemRenderer>());
        flip_emitter_right(ground_slam_hop_first .GetComponent<ParticleSystemRenderer>());
        flip_emitter_right(ground_slam_hop_second.GetComponent<ParticleSystemRenderer>()); 
    }
}
