using Entropek;
using UnityEngine;

public class Rider : Boss<Movement>{
    public void yell_camera_shake() => CameraController.instance.shake_camera(time: 3, amount: 0.75f, lock_shake: false);
    protected Coroutine idle_state;
    
    void Start(){
        AudioManager.load_bank("rider");
        combat.set_moveset("phase_1");
        set_phase_data("phase_1");
        link_events();
    }
    void OnDisable() => unlink_events();
    void OnDestroy(){
        AudioManager.unload_bank("rider");
    }

    // movement functions
    public void forward_strike_lunge()   => movement.dash(flipped == false? Vector2.right : Vector2.left, 20, 0.33f);
    public void signature_lunge()        => movement.dash(flipped == false? Vector2.right : Vector2.left, 20, 0.33f);
    public void signature_jump_back()    => movement.dash(flipped == false? Vector2.left : Vector2.right, 20, 0.33f);
    public void jnd_jump_back()          => movement.dash(flipped == false? Vector2.left : Vector2.right, 20, 0.40f);
    public void jnd_front_leap()         => movement.dash(flipped == false? Vector2.right : Vector2.left, 40, 0.30f);
    public void jump_forward()           => movement.dash(flipped == false? Vector2.right : Vector2.left, 20, 0.5f);
    public void jump_backward()          => movement.dash(flipped == false? Vector2.left : Vector2.right, 20, 0.5f);


    public override void enter_cutscene_state() => idle();
    public override void exit_cutscene_state() => idle(1);

    public void cutscene_yell(){
        no_state();
        animator.Play("RiderYell");
    }

    private void idle(float time) =>
        state_switch(ref idle_state, Util.timer(
            time,
            start_action:()=>idle(),
            time_out:()=>follow_and_attack_state()
        ));

    private void idle(){
        if(idle_state != null)
            StopCoroutine(idle_state);
        animator.Play("RiderIdle");
        no_state();
    }

    void projectile_fired(string x){
        // sound.play_sound("bow_shot");
    }

    void move_direction_changed(Vector2 direction){
        if(combat.is_attacking == true)
            return;
        if(direction == Vector2.left || direction == Vector2.right)
            animator.Play("RiderWalk",0,0);
        else
            animator.Play("RiderIdle");
    }

    void handle_death(){
        movement.halt();
        combat.halt();
        transition_phase();
    }

    protected void link_events(){
        movement.move_direction_changed         += face_direction;
        movement.move_direction_changed         += move_direction_changed;
        health.death                            += handle_death;
        combat.attack_ended                     += idle;
        combat.attack_chosen                    += attack;
        health.damaged                          += sprites.play_damaged_flash;
        ranged.fired                            += projectile_fired;
        link_game_manager();
        link_combat();
        link_movement();
        link_particles();
    }

    protected void unlink_events(){
        movement.move_direction_changed         -= face_direction;
        movement.move_direction_changed         -= move_direction_changed;
        health.death                            -= handle_death;
        combat.attack_ended                     -= idle;
        combat.attack_chosen                    -= attack;
        health.damaged                          -= sprites.play_damaged_flash;
        ranged.fired                            -= projectile_fired;
        unlink_game_manager();
        unlink_combat();
        unlink_movement();
        unlink_particles();
    }
}
