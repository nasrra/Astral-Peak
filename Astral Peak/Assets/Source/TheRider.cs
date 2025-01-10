using Deluz;
using UnityEngine;

public class TheRider : Boss<RiderMovement>{
    public static TheRider instance;
    public void yell_camera_shake() => CameraController.instance.shake_camera(time: 3, amount: 0.75f, lock_shake: false);
    protected Coroutine idle_state;

    void Awake(){
        instance = this;
    }
    void Start(){
        sound.set_functions(new RiderSound(sound));
        combat.set_moveset("phase_1");
        link_events();
        check_game_state();
    }
    void OnDestroy() => unlink_events();

    public override void enter_cutscene_state() => idle();
    public override void exit_cutscene_state() => idle(1);

    public void cutscene_yell(){
        no_state();
        animator.Play("yell");
    }
    public void cutscene_whistle(){
        no_state();
        animator.Play("whistle");
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
        animator.Play("idle");
        no_state();
    }

    void projectile_fired(string x) => sound.play_sound("bow_shot");

    void move_direction_changed(Vector2 direction){
        if(combat.is_attacking == true)
            return;
        if(direction == Vector2.left || direction == Vector2.right)
            animator.Play("run",0,0);
        else
            animator.Play("idle");
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
        flipped_left                            += particles.flip_particles_left;
        flipped_right                           += particles.flip_particles_right;
        ranged.fired                            += projectile_fired;
        link_game_manager();
    }

    protected void unlink_events(){
        movement.move_direction_changed         -= face_direction;
        movement.move_direction_changed         -= move_direction_changed;
        health.death                            -= handle_death;
        combat.attack_ended                     -= idle;
        combat.attack_chosen                    -= attack;
        health.damaged                          -= sprites.play_damaged_flash;
        flipped_left                            -= particles.flip_particles_left;
        flipped_right                           -= particles.flip_particles_right;
        ranged.fired                            -= projectile_fired;
        unlink_game_manager();
    }
}
