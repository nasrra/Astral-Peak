using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheRider : Boss<RiderMovement>{
    public static TheRider instance;
    public void yell_camera_shake() => CameraController.instance.shake_camera(3,0.75f);

    void Awake(){
        instance = this;
        state_switch(idle(1));
        sound.set_functions(new RiderSound(sound));
        link_events();
    }

    void OnDestroy() => unlink_events();

    public override void enter_cutscene_state() => state_switch(lock_idle());
    public override void exit_cutscene_state() => state_switch(idle(1));

    public void switch_to_idle(float x) => state_switch(idle(x));
    IEnumerator idle(float x){
        animator.Play("idle");
        animator.Play("idle");
        yield return new WaitForSeconds(x);
        state_switch(follow());
        yield break;
    }

    public void cutscene_yell_state() => state_switch(cutscene_yell());
    IEnumerator cutscene_yell(){
        animator.Play("yell"); 
        animator.Play("yell"); 
        yield return new WaitForSeconds(3f);
        state_switch(lock_idle());
        yield break;        
    }

    public void cutscene_whistle_state() => state_switch(cutscene_whistle());
    IEnumerator cutscene_whistle(){
        animator.Play("whistle");
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
        movement.move_direction_changed         += face_move_dir;
        health.death                            += kill;
        health.death                            += get_movement().StopAllCoroutines;
        combat.attack_ended                     += switch_to_idle;
        health.damaged                          += sprite.play_damaged_flash;
        flipped_left                            += particles.flip_particles_left;
        flipped_right                           += particles.flip_particles_right;
        ranged.fired                            += projectile_fired;
    }

    protected void unlink_events(){
        movement.move_direction_changed         -= face_move_dir;
        health.death                            -= kill;
        health.death                            -= get_movement().StopAllCoroutines;
        combat.attack_ended                     -= switch_to_idle;
        health.damaged                          -= sprite.play_damaged_flash;
        flipped_left                            -= particles.flip_particles_left;
        flipped_right                           -= particles.flip_particles_right;
        ranged.fired                            -= projectile_fired;
    }
}
