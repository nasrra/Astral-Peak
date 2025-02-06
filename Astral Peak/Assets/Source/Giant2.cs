using UnityEngine;
using Entropek;
using System;

using System.Collections.Generic;
using Entropek.Collections;

public class Giant2 : Boss<Movement>{
    Coroutine idle_state;
    [Header("Giant2")]
    [SerializeField] FinalBossRoomGroundHandler ground_handler;
    [SerializeField] Transform left_hand, right_hand, player_hover_point, start_point;
    [SerializeField] char hand = 'L';
    private int HEAD_LAYER = 0, LHAND_LAYER = 1, RHAND_LAYER = 2;
    // which animation layer to be used for each attack.
    private HashSet<string> 
        head_attacks = new(){
            "Giant2YellProjectiles"
        },
        hand_attacks = new(){
            "Giant2FistSlam"
        };
    // what behaviour occurs when calling said attack.
    private HashSet<string>
        move_to_player = new(){
            "Giant2FistSlam"
        },
        idle_fly = new(){
            "Giant2YellProjectiles"
        };

    void Awake(){
        sound.set_functions(new GiantSound(gameObject));
        state = new StateQueue(this, fly_and_attack_state);
        link_events();
    }
    void Start(){
        set_phase_data("phase_1");
        //switch_phase();
        idle(2);
    }

    private void fly_and_attack_state(){
        animator.Play("Giant2HeadIdle", HEAD_LAYER);
        animator.Play("Giant2HandIdleL", LHAND_LAYER);
        animator.Play("Giant2HandIdleR", RHAND_LAYER);
        combat.chose_attack_state(target);
    }

    protected override void attack(BossAttack attack){
        string animation = attack.animation_id;
        combat.halt();
        if(move_to_player.Contains(animation)){
            state.queue_and_start(time: 120, 
                start_action:()=>{
                    movement.mod_speed(8f);
                    movement.freeform_move_to(player_hover_point);
                    movement.target_reached += move_to_attack;
                }
            );
        }
        else{
            if(hand_attacks.Contains(animation))
                hand_attack(animation);
            else
                state.queue_and_start(time: animator.get_clip_length(animation),start_action:()=>animator.Play(animation,HEAD_LAYER));
        }
    }

    // used for when moving towards to to attack.
    // play attack animation, then move back to starting point.
    private void move_to_attack(){
        movement.target_reached -= move_to_attack;
        string animation = combat.get_chosen_attack().animation_id;
        state.clear();
        state.queue_and_start(time: animator.get_clip_length(animation)+.5f, 
            start_action:()=>{
                movement.freeform_move_to(player_hover_point);
                hand_attack(animation);
            }
        );
        state.queue(time: 120, 
            start_action:()=>{
                movement.freeform_move_to(start_point);
                movement.target_reached += move_to_attack_finished;
            }
        );
    }

    // return to idle when reaching starting point.
    private void move_to_attack_finished(){
        movement.reset_speed();
        state.clear();
        idle(0);
        combat.attack_end();
        movement.figure_eight_state(reverse: false, x_factor:.1f, y_factor:.05f);
        movement.target_reached -= move_to_attack_finished;
    }

    // random left or right hand attack.
    private char choose_hand(){
        hand = UnityEngine.Random.Range(0,2) == 0? 'L' : 'R';
        return hand;
    }
    private void hand_attack(string animation) => animator.Play(animation + choose_hand());
    
    int get_current_ground_piece(){
        int x = -1;
        Collider2D other = Physics2D.OverlapCircle(hand == 'L'?left_hand.position : right_hand.position, .5f, LayersManager.BITWISE_GROUND);
        if(other != null)
            Int32.TryParse(other.name, out x);
        return x;
    }

    public void fist_slam_ground_wave(){
        // left and right
        int ground_piece = get_current_ground_piece();
        ground_handler.start_wave(ground_piece + -1, true, .15f, 400f);
        ground_handler.start_wave(ground_piece + 1, false, .15f, 400f);
    }

    private void idle(float time){
        state.queue_and_start(
            time: time,
            start_action: ()=>{
                animator.Rebind();
                animator.Play("Giant2HeadIdle", HEAD_LAYER);
                animator.Play("Giant2HandIdleL", LHAND_LAYER);
                animator.Play("Giant2HandIdleR", RHAND_LAYER);
            }
        );
    }

    public void projectile_yell_camera_shake() => CameraController.instance.shake_camera(4f,.7f, true);

    protected void link_events(){
        link_game_manager();
        health.death                            += kill;
        health.death                            += movement.StopAllCoroutines;
        combat.attack_chosen                    += attack;
        combat.attack_ended                     += idle;
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
        health.damaged                          -= sprites.play_damaged_flash;
        unlink_particles();
        unlink_combat();
        unlink_movement();
    }
}
