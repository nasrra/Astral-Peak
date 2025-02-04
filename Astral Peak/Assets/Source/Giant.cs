using UnityEngine;
using Entropek;

public class Giant : Boss<Movement>{
    Coroutine idle_state;

    void Awake(){
        sound.set_functions(new GiantSound(sound));
        link_events();
    }
    void Start(){
        set_phase_data("phase_1");
    }

    private void idle(){
        animator.Play("GiantIdle");
        no_state();
    }

    private void stop_idle_loop(){
        if(idle_state != null)
            StopCoroutine(idle_state);
    }

    private void idle(float time){
        stop_idle_loop();
        idle_state = StartCoroutine(Util.timer(
            time,
            start_action:()=>idle(),
            time_out:()=>follow_and_attack_state()
        ));
    }

    void move_direction_changed(Vector2 direction){
        if(combat.is_attacking == true)
            return;
        if(direction == Vector2.left || direction == Vector2.right)
            animator.Play("GiantWalk",0,0);
        else
            animator.Play("GiantIdle");
    }



    protected void link_events(){
        link_game_manager();
        health.death                            += kill;
        health.death                            += movement.StopAllCoroutines;
        combat.attack_chosen                    += attack;
        combat.attack_ended                     += idle;
        movement.move_direction_changed         += face_direction;
        movement.move_direction_changed         += move_direction_changed;
        health.damaged                          += sprites.play_damaged_flash;
        //ranged.fired                          += projectile_fired;
        link_combat();
        link_movement();
        link_particles();
    }

    protected void unlink_events(){
        unlink_game_manager();
        health.death                            -= kill;
        health.death                            -= movement.StopAllCoroutines;
        combat.attack_chosen                    -= attack;
        combat.attack_ended                     -= idle;
        movement.move_direction_changed         -= face_direction;
        movement.move_direction_changed         -= move_direction_changed;
        health.damaged                          -= sprites.play_damaged_flash;
        //ranged.fired                          -= projectile_fired;
        unlink_particles();
        unlink_combat();
        unlink_movement();
    }
}
