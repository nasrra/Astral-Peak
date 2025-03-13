using UnityEngine;
using Entropek;
using System;

public class Giant1 : Boss<Movement>{
    Coroutine idle_state;
    [Header("Giant1")]
    [SerializeField] FinalBossRoomGroundHandler ground_handler;
    [SerializeField] Transform hammer_ground_check;

    void Awake(){
        link_events();
    }
    void Start(){
        set_phase_data("phase_1");
        //idle(3);
    }

    void OnDestroy() => unlink_events();

    protected override void exit_cutscene_state(){
        idle(1);
    }

    protected override void enter_cutscene_state(){
        no_state();
    }

    private void idle(){
        animator.Play("Giant1Idle");
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
            animator.Play("Giant1Walk",0,0);
        else
            animator.Play("Giant1Idle");
    }

    // ground piece we are standing on.
    int get_current_standing_ground_piece(){
        int x = -1;
        Collider2D other = Physics2D.OverlapCircle(transform.position, .5f, LayersManager.BITWISE_GROUND);
        if(other != null)
            Int32.TryParse(other.name, out x);
        return x;
    }

    // ground piece hammer collides with.
    int get_current_hammer_ground_piece(){
        int x = -1;
        // cahnge to raycast 2d down.
        Collider2D other = Physics2D.OverlapCircle(hammer_ground_check.position, .5f, LayersManager.BITWISE_GROUND);
        if(other != null)
            Int32.TryParse(other.name, out x);
        return x;
    }

    // Camera
    public void down_slam_camera_shake() => CameraController.instance.shake_camera(.35f,.75f,false);
    public void jump_away_camera_shake() => CameraController.instance.shake_camera(.4f,.8f,false);
    public void yell_camera_shake() => CameraController.instance.shake_camera(3f,.8f, true);
    public void death_camera_shake() => CameraController.instance.shake_camera(.5f,.8f, true);

    // ground waves.
    public void down_slam_ground_wave(){
        int ground_piece = get_current_hammer_ground_piece();
        ground_handler.start_wave(ground_piece + -1, true, .15f, 400f);
        ground_handler.start_wave(ground_piece + 1, false, .15f, 400f);        
    }
    public void round_slam_left_ground_wave(){
        int ground_piece = get_current_hammer_ground_piece();
        ground_handler.start_wave(ground_piece + -1, true, .15f, 400f);
        ground_handler.start_wave(ground_piece + 1, false, .15f, 400f);
        // ground_handler.start_wave(get_current_ground_piece() + (flipped==true?-1:1), flipped, .15f, 400f);
    }
    public void round_slam_right_ground_wave(){        
        int ground_piece = get_current_hammer_ground_piece();
        ground_handler.start_wave(ground_piece + -1, true, .15f, 400f);
        ground_handler.start_wave(ground_piece + 1, false, .15f, 400f);
        // ground_handler.start_wave(get_current_ground_piece() + (flipped==true?1:-1), !flipped, .15f, 400f);
    }
    public void jump_away_ground_wave(){
        // left and right
        int ground_piece = get_current_standing_ground_piece();
        ground_handler.start_wave(ground_piece + -1, true, .15f, 400f);
        ground_handler.start_wave(ground_piece + 1, false, .15f, 400f);
    }
    public void start_geysers() => ground_handler.use_geysers();

    protected override void death_start(){
        StopAllCoroutines();
        no_state();
        stop_all();
        enable_body_colliders(0);
        movement.zero_velocity(); // stop velocity in case the boss is dashing.
        animator.Play("Giant1Death",0,0);
        base.death_start();
        StartCoroutine(Util.timer(
            animator.get_clip_length("Giant1Death")+3,
            time_out:()=>{
                unlink_events();
                gameObject.SetActive(false);
                base.death_complete();//
            }
        ));
    }

    //movement.
    public void jump_forward() => movement.dash(flipped == false? Vector2.right : Vector2.left, 12.5f, 0.5f);
    public void jump_backward() => movement.dash(flipped == true? Vector2.right : Vector2.left, 12.5f, 0.5f);
    public void three_piece_jump() => movement.dash(flipped == false? Vector2.right : Vector2.left, 18f, 0.5f);
    public void three_piece_lunge() => movement.dash(flipped == false? Vector2.right : Vector2.left, 18.5f, 0.3f);

    protected void projectile_fired(string holster_id){
        sound.play_diegetic_one_shot("water_bubble");
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
