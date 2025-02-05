using UnityEngine;
using Entropek;
using System;

public class Giant : Boss<Movement>{
    Coroutine idle_state;
    [Header("TheGiant")]
    [SerializeField] FinalBossRoomGroundHandler ground_handler;

    void Awake(){
        sound.set_functions(new GiantSound(sound));
        link_events();
    }
    void Start(){
        set_phase_data("phase_1");
        //switch_phase();
        idle(3);
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

    int get_current_ground_piece(){
        int x = -1;
        Collider2D other = Physics2D.OverlapCircle(transform.position, .5f, LayersManager.BITWISE_GROUND);
        if(other != null)
            Int32.TryParse(other.name, out x);
        return x;
    }

    public void down_slam_ground_wave() => ground_handler.start_wave(get_current_ground_piece() + (flipped==true?-1:1), flipped, .15f, 400f);
    public void round_slam_left_ground_wave() => ground_handler.start_wave(get_current_ground_piece() + (flipped==true?-1:1), flipped, .15f, 400f);
    public void round_slam_right_ground_wave() => ground_handler.start_wave(get_current_ground_piece() + (flipped==true?1:-1), !flipped, .15f, 400f);
    public void down_slam_camera_shake() => CameraController.instance.shake_camera(.35f,.75f,false);
    public void jump_away_ground_wave(){
        // left and right
        ground_handler.start_wave(get_current_ground_piece() + -1, true, .15f, 400f);
        ground_handler.start_wave(get_current_ground_piece() + 1, false, .15f, 400f);
    }
    public void jump_away_camera_shake() => CameraController.instance.shake_camera(.4f,.8f,false);

    //movement.
    public void jump_forward() => movement.dash(flipped == false? Vector2.right : Vector2.left, 12.5f, 0.5f);
    public void jump_backward() => movement.dash(flipped == true? Vector2.right : Vector2.left, 12.5f, 0.5f);

    protected void link_events(){
        link_game_manager();
        health.death                            += kill;
        health.death                            += movement.StopAllCoroutines;
        combat.attack_chosen                    += attack;
        combat.attack_ended                     += idle;
        movement.move_direction_changed         += face_direction;
        movement.move_direction_changed         += move_direction_changed;
        health.damaged                          += sprites.play_damaged_flash;
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
        unlink_particles();
        unlink_combat();
        unlink_movement();
    }
}
