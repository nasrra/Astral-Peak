using UnityEngine;
using Entropek;
using System;

using System.Collections.Generic;
using Entropek.Collections;

public class Giant2 : Boss<Movement>{
    Coroutine idle_state;
    [Header("Giant2")]
    [SerializeField] FinalBossRoomGroundHandler ground_handler;
    private int HEAD_LAYER = 0, LHAND_LAYER = 1, RHAND_LAYER = 2;
    // which animation layer to be used for each attack.
    private HashSet<string> 
        head_attacks = new(){
            "Giant2YellProjectiles"
        },
        hand_attacks = new(){

        };
    // what behaviour occurs when calling said attack.
    private HashSet<string>
        move_to = new(){

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
        idle(1);
        movement.figure_eight_state(reverse: false, x_factor:.1f, y_factor:.05f);
    }

    private void fly_and_attack_state(){
        animator.Play("Giant2Idle", HEAD_LAYER);
        animator.Play("Giant2Idle", LHAND_LAYER);
        animator.Play("Giant2Idle", RHAND_LAYER);
        combat.chose_attack_state(target);
    }

    protected override void attack(BossAttack attack){
        string animation = attack.animation_id;
        if(move_to.Contains(animation)){
            // do stuff.
        }
        if(hand_attacks.Contains(animation))
            state.queue_and_start(time: animator.get_clip_length(animation),start_action:()=>animator.Play(animation,UnityEngine.Random.Range(1,3)));
        else
            state.queue_and_start(time: animator.get_clip_length(animation),start_action:()=>animator.Play(animation,HEAD_LAYER));
        combat.halt();
    }

    private void idle(float time){
        state.queue_and_start(
            time: time,
            start_action: ()=>{
                animator.Rebind();
                animator.Play("Giant2Idle", HEAD_LAYER);
                animator.Play("Giant2Idle", LHAND_LAYER);
                animator.Play("Giant2Idle", RHAND_LAYER);
            }
        );
    }

    int get_current_ground_piece(){
        int x = -1;
        Collider2D other = Physics2D.OverlapCircle(transform.position, .5f, LayersManager.BITWISE_GROUND);
        if(other != null)
            Int32.TryParse(other.name, out x);
        return x;
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
