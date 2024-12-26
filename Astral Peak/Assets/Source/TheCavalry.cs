using System.Collections;
using Deluz;
using UnityEngine;

public class TheCavalry : Boss<CavalryMovement>{
    public static TheCavalry instance;
    
    void Awake(){
        instance = this;
        idle(2);
        sound.set_functions(new CavalrySound(sound));
        link_events();
    } 

    void OnDestroy() {
        unlink_events();
        StopAllCoroutines();
    }

    // states: 
    public override void enter_cutscene_state() => idle();
    public override void exit_cutscene_state() => idle(1);

    public override void kill(){
        animator.Play("death");
        StartCoroutine(Util.timer(
            animator.GetCurrentAnimatorClipInfo(0).Length + 3,
            start_action:()=>{
                enable_body_colliders(0);
                sprite.play_death_effect(2.25f);
                disable_components();
                movement.zero_velocity(); // stop velocity in case the boss is dashing.
                particles.stop_all_particles();
            },
            time_out:()=>{
                AudioManager.stop_music();
                UiManager.instance.play_enemy_vanquished();
                gameObject.SetActive(false);
            }
        ));
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

    private void idle(float time) =>
        StartCoroutine(Util.timer(
            time,
            start_action:()=> idle(),
            time_out:()=>follow_and_attack_state()
        ));

    private void idle(){
        animator.Play("idle");
        no_state();
    }

    void projectile_fired(string x) => sound.play_sound("bow_shot");

    void move_direction_changed(Vector2 direction){
        if(combat.is_attacking == true)
            return;
        if(direction == Vector2.left || direction == Vector2.right)
            animator.Play("run");
        else
            animator.Play("idle");
    }

    protected void link_events(){
        health.death                            += kill;
        health.death                            += movement.StopAllCoroutines;
        combat.attack_ended                     += idle;
        movement.move_direction_changed         += face_direction;
        movement.move_direction_changed         += move_direction_changed;
        flipped_left                            += particles.flip_particles_left;
        flipped_right                           += particles.flip_particles_right;
        health.damaged                          += sprite.play_damaged_flash;
        ranged.fired                            += projectile_fired;
    }

    protected void unlink_events(){
        health.death                            -= kill;
        health.death                            -= movement.StopAllCoroutines;
        combat.attack_ended                     -= idle;
        movement.move_direction_changed         -= face_direction;
        movement.move_direction_changed         -= move_direction_changed;
        flipped_left                            -= particles.flip_particles_left;
        flipped_right                           -= particles.flip_particles_right;
        health.damaged                          -= sprite.play_damaged_flash;
        ranged.fired                            -= projectile_fired;
    }
}
