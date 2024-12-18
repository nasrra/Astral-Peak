using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheCavalry : Boss<CavalryMovement>{
    public static TheCavalry instance;

    // Start is called before the first frame update
    [SerializeField] AnimationEvent animation_event;
    [SerializeField] ParticlesHandler particles;
    [SerializeField] CavalryRangedCombatHandler ranged;
    [SerializeField] MeleeHolsterHandler melee;
    [SerializeField] public CavalryAudio sound;
    [SerializeField] List<Collider2D> body_colliders;
    Dictionary<string, Action> animation_events;

    void Awake(){
        instance = this;
        //state_switch(lock_idle());
        create_animation_events();
        state_switch(idle(1));
        link_events();
    } 

    void OnDestroy() {
        unlink_events();
        StopAllCoroutines();
    }

    public void switch_to_idle(float x) => state_switch(idle(x));

    // states: 
    public override void enter_cutscene_state() => state_switch(lock_idle());
    public override void exit_cutscene_state() => state_switch(idle(1));

    public override void kill(){
        base.kill();
        state_switch(death_state());
    }

    private void handle_animation_events(string x) => animation_events[x]();
    private void create_animation_events(){
        animation_events = new Dictionary<string, Action>(){
            // Sound:
            {"play_melee_sound",                    ()=>sound.sword_strike()},
            {"play_footstep_sound",                 ()=>sound.footstep()},
            {"play_howl_sound",                     ()=>sound.howl()},
            {"play_bark_sound",                     ()=>sound.bark()},
            {"play_slam_impact_sound",              ()=>sound.ground_slam_impact()},
            {"play_slam_hop_sound",                 ()=>sound.ground_slam_hop()},
            {"play_death_explosion_sound",          ()=>sound.magic_explosion()},

            // Particles:
            {"emit_front_footstep_particles",       ()=>particles.play("front_footstep")},
            {"emit_back_footstep_particles",        ()=>particles.play("back_footstep")},
            {"emit_jump_particles",                 ()=>particles.play("jump")},
            {"emit_slam_hop_1_particles",           ()=>particles.emit("slam_hop_1",1)},
            {"emit_slam_hop_2_particles",           ()=>particles.emit("slam_hop_2",1)},
            {"emit_slam_impact_particles",          ()=>particles.play("slam_impact")},
            {"emit_front_strike_particles",         ()=>particles.emit("front_strike",1)},
            {"emit_death_ambience_particles",       ()=>particles.play("death_ambience")},
            {"stop_death_ambience_particles",       ()=>particles.stop("death_ambience")},
            {"emit_death_explosion_particles",      ()=>particles.play("death_explosion")},
            {"emit_snow_dash_particles",            ()=>particles.play("snow_dash")},
            {"stop_snow_dash_particles",            ()=>particles.stop("snow_dash")},
            {"emit_bite_particles",                 ()=>{particles.emit("bite_1",1); particles.emit("bite_2",1);}},
            {"emit_back_slash_front_particles",     ()=>particles.emit("back_slash_front",1)},
            {"emit_back_slash_back_particles",      ()=>particles.emit("back_slash_back",1)},

            // Movement:
            {"flip_to_target",                      ()=>flip_to_target()},
            {"jump_away_movement",                  ()=>movement.jump_away_dash()},
            {"bite_lunge_movement",                 ()=>movement.bite_lunge()},
            {"back_slash_front_movement",           ()=>movement.back_strike_forward_leap()},
            {"back_slash_back_movement",            ()=>movement.back_strike_backward_jump()},

            // Melee:
            {"enable_front_strike_hurtbox",         ()=>melee.toggle_hurt_box("front_strike", true)},
            {"disable_front_strike_hurtbox",        ()=>melee.toggle_hurt_box("front_strike", false)},
            {"enable_bite_hurtbox",                 ()=>melee.toggle_hurt_box("bite",true)},
            {"disable_bite_hurtbox",                ()=>melee.toggle_hurt_box("bite",false)},
            {"enable_back_slash_front_hurtbox",     ()=>melee.toggle_hurt_box("back_slash_front",true)},
            {"disable_back_slash_front_hurtbox",    ()=>melee.toggle_hurt_box("back_slash_front",false)},
            {"enable_back_slash_back_hurtbox",      ()=>melee.toggle_hurt_box("back_slash_back",true)},
            {"disable_back_slash_back_hurtbox",     ()=>melee.toggle_hurt_box("back_slash_back",false)},

            // Ranged:
            {"jump_away_arrow",                     ()=>ranged.jump_arrow_fire_once()},
            {"howl_arrow",                          ()=>ranged.howl_arrows()},
            {"slam_projectiles",                    ()=>ranged.ground_slams()},
            // Combat
            {"attack_end",                          ()=>combat.attack_end()},
            
            // camera.
            {"slam_camera_adjust", ()=>{
                CameraController.instance.move_vertical_state(7, 4f);
                CameraController.instance.zoom_out_state(14, 2f);
            }},
            {"slam_camera_reset",()=>{
                CameraController.instance.reset_offset_state(32f);
                CameraController.instance.reset_zoom_state(16f);
            }},
            {"slam_camera_shake",                   ()=>CameraController.instance.shake_camera(0.15f, 1f)},
            {"death_camera_shake",                  ()=>CameraController.instance.shake_camera(0.25f, 1f)},
        };
    }



    IEnumerator death_state(){
        foreach(Collider2D c in body_colliders)
            c.enabled = false;
        animator.Play("death");
        sprite.play_death_effect(2.25f);
        disable_components();
        movement.zero_velocity(); // stop velocity in case the boss is dashing.
        particles.stop_all_particles();
        yield return new WaitForSeconds(6);
        AudioManager.stop_music();
        UiManager.instance.play_enemy_vanquished();
        gameObject.SetActive(false);
        yield break;
    }

    void disable_components(){
        particles.StopAllCoroutines();
        particles.enabled = false;
        ranged.StopAllCoroutines();
        ranged.enabled = false;
        movement.StopAllCoroutines();
        movement.enabled = false;
        melee.StopAllCoroutines();
        melee.enabled = false;
        combat.StopAllCoroutines();
        combat.enabled = false;
    }

    IEnumerator idle(float x){ 
        animator.Play("idle");
        yield return new WaitForSeconds(x);
        state_switch(follow());
        yield break;
    }

    IEnumerator lock_idle(){
        animator.Play("idle");
        yield break;
    }

    protected override IEnumerator follow(){
        animator.Play("run");
        state_switch(base.follow());
        yield break;
    }

    protected void link_events(){
        health.death                            += kill;
        health.death                            += get_movement().StopAllCoroutines;
        combat.attack_ended                     += switch_to_idle;
        ranged.arrow_fired                      += sound.bow_shot;
        get_movement().move_direction_changed   += face_move_dir;
        flipped_left                            += particles.flip_left;
        flipped_right                           += particles.flip_right;
        health.damaged                          += sprite.play_damaged_flash;
        link_animation_event();
    }

    protected void unlink_events(){
        health.death                            -= kill;
        health.death                            -= get_movement().StopAllCoroutines;
        combat.attack_ended                     -= switch_to_idle;
        ranged.arrow_fired                      -= sound.bow_shot;
        get_movement().move_direction_changed   -= face_move_dir;
        flipped_left                            -= particles.flip_left;
        flipped_right                           -= particles.flip_right;
        health.damaged                          -= sprite.play_damaged_flash;
        unlink_animation_event();
    }

    void link_animation_event() => animation_event.signal += handle_animation_events;
    void unlink_animation_event() => animation_event.signal -= handle_animation_events;
}
