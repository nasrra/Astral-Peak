using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheCavalry : Boss<CavalryMovement>{
    public static TheCavalry instance;
    
    void Awake(){
        instance = this;
        //state_switch(lock_idle());
        state_switch(idle(1));
        sound.set_functions(new CavalrySound(sound));
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

    IEnumerator death_state(){
        enable_body_colliders(false);
        animator.Play("death");
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
        animator.Play("idle");
        yield return new WaitForSeconds(x);
        state_switch(follow());
        yield break;
    }

    IEnumerator lock_idle(){
        animator.Play("idle");
        animator.Play("idle");
        yield break;
    }

    protected override IEnumerator follow(){
        animator.Play("run");
        animator.Play("run");
        state_switch(base.follow());
        yield break;
    }

    void projectile_fired(string x) => sound.play_sound("bow_shot");

    protected void link_events(){
        health.death                            += kill;
        health.death                            += get_movement().StopAllCoroutines;
        combat.attack_ended                     += switch_to_idle;
        get_movement().move_direction_changed   += face_move_dir;
        flipped_left                            += particles.flip_particles_left;
        flipped_right                           += particles.flip_particles_right;
        health.damaged                          += sprite.play_damaged_flash;
        ranged.fired                            += projectile_fired;
    }

    protected void unlink_events(){
        health.death                            -= kill;
        health.death                            -= get_movement().StopAllCoroutines;
        combat.attack_ended                     -= switch_to_idle;
        get_movement().move_direction_changed   -= face_move_dir;
        flipped_left                            -= particles.flip_particles_left;
        flipped_right                           -= particles.flip_particles_right;
        health.damaged                          -= sprite.play_damaged_flash;
        ranged.fired                            -= projectile_fired;
    }
}
