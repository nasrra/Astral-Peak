using Deluz;
using UnityEngine;

public class TheRider : Boss<RiderMovement>{
    public static TheRider instance;
    public void yell_camera_shake() => CameraController.instance.shake_camera(time: 3, amount: 0.75f, lock_shake: false);
    protected Coroutine idle_state;

    void Awake(){
        instance = this;
        sound.set_functions(new RiderSound(sound));
        link_events();
        idle(1);
    }
    void Start(){
        select_phase(phase);
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

    protected void link_events(){
        movement.move_direction_changed         += face_direction;
        movement.move_direction_changed         += move_direction_changed;
        health.death                            += kill;
        health.death                            += movement.StopAllCoroutines;
        combat.attack_ended                     += idle;
        combat.attack_chosen                    += attack;
        health.damaged                          += sprites.play_damaged_flash;
        flipped_left                            += particles.flip_particles_left;
        flipped_right                           += particles.flip_particles_right;
        ranged.fired                            += projectile_fired;
        phase_selected                          += combat.set_moveset;
    }

    protected void unlink_events(){
        movement.move_direction_changed         -= face_direction;
        movement.move_direction_changed         -= move_direction_changed;
        health.death                            -= kill;
        health.death                            -= movement.StopAllCoroutines;
        combat.attack_ended                     -= idle;
        combat.attack_chosen                    -= attack;
        health.damaged                          -= sprites.play_damaged_flash;
        flipped_left                            -= particles.flip_particles_left;
        flipped_right                           -= particles.flip_particles_right;
        ranged.fired                            -= projectile_fired;
        phase_selected                          += combat.set_moveset;
    }
}
