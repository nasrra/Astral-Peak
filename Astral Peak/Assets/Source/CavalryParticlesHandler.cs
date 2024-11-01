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
        front_strike            .GetComponent<ParticleSystemRenderer>().flip = left;
        back_strike_backward    .GetComponent<ParticleSystemRenderer>().flip = left;
        back_strike_forward     .GetComponent<ParticleSystemRenderer>().flip = left;
        top_bite                .GetComponent<ParticleSystemRenderer>().flip = left;
        bottom_bite             .GetComponent<ParticleSystemRenderer>().flip = left;
        ground_slam_hop_first   .GetComponent<ParticleSystemRenderer>().flip = left;
        ground_slam_hop_second  .GetComponent<ParticleSystemRenderer>().flip = left;
    }

    public override void flip_right(){
        front_strike            .GetComponent<ParticleSystemRenderer>().flip = right;
        back_strike_backward    .GetComponent<ParticleSystemRenderer>().flip = right;
        back_strike_forward     .GetComponent<ParticleSystemRenderer>().flip = right;
        top_bite                .GetComponent<ParticleSystemRenderer>().flip = right;
        bottom_bite             .GetComponent<ParticleSystemRenderer>().flip = right;
        ground_slam_hop_first   .GetComponent<ParticleSystemRenderer>().flip = right;
        ground_slam_hop_second  .GetComponent<ParticleSystemRenderer>().flip = right;   
    }
}
