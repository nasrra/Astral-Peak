using System;
using AYellowpaper.SerializedCollections;
using Entropek.Collections;
using UnityEngine;

public class Giant2Hand : MonoBehaviour{
    [SerializeField] SerializedDictionary<string, Transform> move_to_point = new SerializedDictionary<string, Transform>();
    [field: SerializeField] public AnimatorOverride animator {get; private set;}
    [field: SerializeField] public BossSpriteHandler sprite {get; private set;}
    [SerializeField] FinalBossRoomGroundHandler ground_handler;
    [SerializeField] SludgeBeam finger_beam;
    [SerializeField] Movement movement;
    [SerializeField] Transform head_follower;
    [SerializeField] AudioPlayer audio_player;
    Transform move_to_target;
    private StateQueue state;
    string current_animation;

    void Awake(){
        state = new StateQueue(this, idle);
        link();
    }

    void Oestroy(){
        unlink();
    }

    public void idle(){
        animator.Play("GiantHandIdle");
        movement.freeform_move_to(head_follower);
    }

    public void attack(string animation){
        current_animation = animation;
        if(move_to_point.ContainsKey(animation))
            state.queue_and_start(
                time: 120, 
                start_action:()=> move_to_point_begin(move_to_point[animation])
            );
        animator.Play(animation);
    }

    private void move_to_point_begin(Transform target){
        move_to_target = target;
            state.queue_and_start(time: 120, 
                start_action:()=>{
                    movement.mod_speed(8f);
                    movement.freeform_approach_to(move_to_target);
                    movement.target_reached += move_to_point_attack;
                }
            );        
    }

    private void move_to_point_attack(){
        movement.target_reached -= move_to_point_attack;
        state.clear();
        state.queue_and_start(time: animator.get_clip_length(current_animation), 
            start_action:()=>{
                movement.freeform_move_to(move_to_target);
                animator.Play(current_animation);
            },
            time_out:()=>animator.Play("GiantHandIdle")
        );
        state.queue(time: 120, 
            start_action:()=>{
                movement.freeform_move_to(head_follower);
                movement.target_reached += move_to_attack_finished;
            }
        );
    }

    private void move_to_attack_finished(){
        state.clear();
        movement.target_reached -= move_to_attack_finished;
    }

    public void turn_on_finger_beam() => finger_beam.turn_on();
    public void turn_off_finger_beam() => finger_beam.turn_off();

    int get_current_ground_piece(){
        int x = -1;
        // cahnge to raycast 2d down.
        Collider2D other = Physics2D.OverlapCircle(transform.position, .5f, LayersManager.BITWISE_GROUND);
        if(other != null)
            Int32.TryParse(other.name, out x);
        return x;
    }

    public void fist_slam_camera_shake()    => CameraController.instance.shake_camera(.8f,.65f,false);
    public void clap_camera_shake()         => CameraController.instance.shake_camera(.8f,.5f,false);


    public void fist_slam_ground_wave(){
        // left and right
        int ground_piece = get_current_ground_piece();
        ground_handler.start_wave(ground_piece + -1, true, .15f, 400f);
        ground_handler.start_wave(ground_piece + 1, false, .15f, 400f);
    }

    public void death(){
        throw new Exception("hand does not have death yet!");
    }

    protected void entered_game_state(GameState state){
        if(state == GameState.CUTSCENE)
            this.state.clear_and_stop();
    }

    protected void exited_game_state(GameState state){
        if(state == GameState.CUTSCENE)
            this.state.start();
    }

    void link(){
        GameManager.entered_game_state += entered_game_state;
        GameManager.exited_game_state += exited_game_state;
    }

    void unlink(){
        GameManager.entered_game_state -= entered_game_state;
        GameManager.exited_game_state -= exited_game_state;
    }
}
