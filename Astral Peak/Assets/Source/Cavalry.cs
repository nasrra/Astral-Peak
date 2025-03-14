using Entropek;
using UnityEngine;

public class Cavalry : Boss<CavalryMovement>{
    Coroutine idle_state;

    void Awake(){
        //idle(2);
        entered_game_state(GameManager.get_state());
        link_events();
    } 

    void Start(){
        set_phase_data("phase_1");
    }

    void OnDestroy() {
        unlink_events();
        StopAllCoroutines();
    }

    // states: 
    protected override void enter_cutscene_state(){
        stop_idle_loop();    
        idle();
    }
    protected override void exit_cutscene_state(){
        idle(1);
    }   

    protected override void death_start(){
        animator.PlayInstant("WolfDeath");
        no_state();
        if(idle_state != null)
            StopCoroutine(idle_state);
        enable_body_colliders(0);
        stop_all();
        movement.zero_velocity(); // stop velocity in case the boss is dashing.
        particles.stop_all_particles();
        sprites.play_death_effect(2f);
        base.death_start();
        StartCoroutine(Util.timer(
            animator.get_clip_length("WolfDeath")+3,
            time_out:()=>{
                // AudioManager.stop_music();
                UiManager.instance.play_enemy_vanquished();
                gameObject.SetActive(false);
                base.death_complete();//
            }
        ));
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

    private void idle(){
        animator.Play("WolfIdle");
        no_state();
    }

    void projectile_fired(string x){
        sound.play_diegetic_one_shot("rider_bow_shot");
    } 

    void move_direction_changed(Vector2 direction){
        if(combat.is_attacking == true)
            return;
        if(direction == Vector2.left || direction == Vector2.right)
            animator.CrossFade("WolfRun",0.2f,0,0);
        else
            animator.CrossFade("WolfIdle",0.2f,0,0);
    }

    // movement:
    public void back_strike_forward_leap()  => movement.dash(flipped == false? Vector2.right : Vector2.left, 20, 0.75f);
    public void back_strike_backward_jump() => movement.dash(flipped == false? Vector2.left : Vector2.right, 20, 0.6f);
    public void jump_away_dash()            => movement.dash(flipped == false? Vector2.left : Vector2.right, 15, 0.55f);
    public void bite_lunge()                => movement.dash(flipped == false? Vector2.right : Vector2.left, 20, 0.2f);


    protected void link_events(){
        link_game_manager();
        health.death                            += kill;
        health.death                            += movement.StopAllCoroutines;
        combat.attack_chosen                    += attack;
        combat.attack_ended                     += idle;
        movement.move_direction_changed         += face_direction;
        movement.move_direction_changed         += move_direction_changed;
        health.damaged                          += sprites.play_damaged_flash;
        ranged.fired                            += projectile_fired;
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
        ranged.fired                            -= projectile_fired;
        unlink_particles();
        unlink_combat();
        unlink_movement();
    }
}
