using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheRider : Boss<RiderMovement>{
    public static TheRider instance;
    [SerializeField] AnimationEvent animation_event;
    [SerializeField] RiderParticlesHandler particles;
    [SerializeField] RiderRangedCombat ranged;
    [SerializeField] MeleeHolsterHandler melee;
    [SerializeField] RiderAudio sound;
    Dictionary<string, Action> animation_events;
    public void yell_camera_shake() => CameraController.instance.shake_camera(3,0.75f);

    void Awake() => create_animation_events();

    void OnEnable(){
        instance = this;
        state_switch(idle(1));
    }

    void Start() => link_events();
    void OnDestroy() => unlink_events();

    public override void enter_cutscene_state() => state_switch(lock_idle());
    public override void exit_cutscene_state() => state_switch(idle(1));

    public void switch_to_idle(float x) => state_switch(idle(x));
    IEnumerator idle(float x){
        animator.Play("idle");
        yield return new WaitForSeconds(x);
        state_switch(follow());
        yield break;
    }

    public void cutscene_yell_state() => state_switch(cutscene_yell());
    IEnumerator cutscene_yell(){
        animator.Play("yell"); 
        yield return new WaitForSeconds(3f);
        state_switch(lock_idle());
        yield break;        
    }

    public void cutscene_whistle_state() => state_switch(cutscene_whistle());
    IEnumerator cutscene_whistle(){
        animator.Play("whistle");
        yield return new WaitForSeconds(2.1f);
        state_switch(lock_idle());
        yield break;
    }

    IEnumerator yell(){
        animator.Play("yell");
        yield return new WaitForSeconds(3);
        state_switch(idle(1));
        yield break;
    }

    IEnumerator lock_idle(){
        animator.Play("idle");
        ParticleSystem p = new ParticleSystem();
        yield break;
    }

    protected override IEnumerator follow(){
        animator.Play("run");
        state_switch(base.follow());
        yield break;
    }

    private void handle_animation_events(string x) => animation_events[x]();
    private void create_animation_events(){
        animation_events = new Dictionary<string, Action>(){
            //Combat:
            {"enable_signature_strike_hurtbox",      ()=>melee.toggle_hurt_box("signature", true)},
            {"disable_signature_strike_hurtbox",     ()=>melee.toggle_hurt_box("signature", false)},
            {"enable_jnd_strike_hurtbox",            ()=>melee.toggle_hurt_box("jump_and_dash", true)},
            {"disable_jnd_strike_hurtbox",           ()=>melee.toggle_hurt_box("jump_and_dash", false)},
            {"attack_end",                           ()=>combat.attack_end()}, 
            {"round_shot_arrow_1",                   ()=>ranged.fire_round_shot_arrow(0)},
            {"round_shot_arrow_2",                   ()=>ranged.fire_round_shot_arrow(1)},
            {"round_shot_arrow_3",                   ()=>ranged.fire_round_shot_arrow(2)},
            {"round_shot_arrow_4",                   ()=>ranged.fire_round_shot_arrow(3)},
            {"round_shot_arrow_5",                   ()=>ranged.fire_round_shot_arrow(4)},
            {"signature_arrow",                      ()=>ranged.fire_signature_arrow()},
            {"flip_to_target",                       ()=>flip_to_target()},
            //Particles:
            {"emit_jump_particles",                  ()=>particles.play("jump")},
            {"emit_magic_dash_particles",            ()=>particles.play("magic_dash")},
            {"emit_snow_dash_particles",             ()=>particles.play("snow_dash")},
            {"stop_snow_dash_particles",             ()=>particles.stop("snow_dash")},
            {"emit_jnd_strike_particles",            ()=>particles.play("jnd_strike")},
            {"emit_footstep_particles",              ()=>particles.play("footstep")},
            {"emit_signature_strike_particles",      ()=>particles.emit("signature_strike",1)},
            {"emit_signature_slam_particles",        ()=>particles.play("signature_slam")},
            //Movement:
            {"jnd_jump_back",                        ()=>movement.jnd_jump_back()},
            {"jnd_front_leap",                       ()=>movement.jnd_front_leap()},
            {"forward_strike_lunge",                 ()=>movement.forward_strike_lunge()},
            {"signature_strike_lunge",               ()=>movement.signature_strike_lunge()},
            //Sound:           
            {"play_dash_sound",                      ()=>sound.magic()},
            {"play_footstep_sound",                  ()=>sound.footstep()},
            {"play_melee_sound",                     ()=>sound.melee_attack()},
            {"play_whistle_sound",                   ()=>sound.whistle()},
        };
    }

    protected void link_events(){
        movement.move_direction_changed         += face_move_dir;
        health.death                            += kill;
        health.death                            += get_movement().StopAllCoroutines;
        combat.attack_ended                     += switch_to_idle;
        health.damaged                          += sprite.play_damaged_flash;
        flipped_left                            += particles.flip_left;
        flipped_right                           += particles.flip_right;
        ranged.arrow_fired                      += sound.arrow_shot;
        link_animation_event();
    }

    protected void unlink_events(){
        movement.move_direction_changed         -= face_move_dir;
        health.death                            -= kill;
        health.death                            -= get_movement().StopAllCoroutines;
        combat.attack_ended                     -= switch_to_idle;
        health.damaged                          -= sprite.play_damaged_flash;
        flipped_left                            -= particles.flip_left;
        flipped_right                           -= particles.flip_right;
        ranged.arrow_fired                      -= sound.arrow_shot;
        unlink_animation_event();
    }

    protected void link_animation_event() => animation_event.signal += handle_animation_events;
    protected void unlink_animation_event() => animation_event.signal -= handle_animation_events;
}
