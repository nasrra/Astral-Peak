using UnityEngine;
using Entropek;
using System;
using System.Collections.Generic;
using Entropek.Collections;
using AYellowpaper.SerializedCollections;

public class Giant2 : Boss<Movement>{
    [Header("Giant2")]
    [SerializeField] SerializedDictionary<string, Transform> move_to_point; // points attacks can move to.
    [SerializeField] Giant2Hand left_hand, right_hand;
    // which animation layer to be used for each attack.
    private HashSet<string> 
        head_attacks = new(){
            "Giant2Projectiles"
        },
        single_hand_attacks = new(){
            "Giant2Slam"
        },
        double_hand_synced_attacks = new(){
            "Giant2Gun",
            "Giant2Clap",
            "Giant2Geyser"
        },
        double_hand_asynced_attacks = new(){
            "Giant2MultiSlam",
        };
    // what behaviour occurs when calling said attack.
    private HashSet<string>
        move_to_player = new(){
            "Giant2Slam",
            "Giant2Clap"
        };
        //idle_fly = new(){
        //    "Giant2Projectiles",
        //    "Giant2MultiSlam"
        //};
    [SerializeField] Transform head, player_hover_point, start_point, move_to_target;
    bool is_idle_flying = false;

    void OnEnable(){
        state = new StateQueue(this, fly_and_attack_state);
        link_events();
    }
    void Start(){
        set_phase_data("phase_1");
        //idle(1);
    }
    void OnDisable() => unlink_events();

    protected override void enter_cutscene_state(){
        this.state.clear_and_stop();
    }

    protected override void exit_cutscene_state(){
        idle(1);
    }

    private void fly_and_attack_state(){
        animator.CrossFade("Giant2IdleHead",0.1f,0,0);
        left_hand.animator.CrossFade("Giant2IdleHand",0.1f,0,0);
        right_hand.animator.CrossFade("Giant2IdleHand",0.1f,0,0);
        combat.chose_attack_state(target);
    }

    protected override void attack(BossAttack attack){
        string animation = attack.animation_id;
        combat.halt();
        if(move_to_player.Contains(animation)){
            is_idle_flying = false;
            state.queue_and_start(
                time: 120, 
                start_action:()=> move_to_player_begin(player_hover_point)
            );
        }
        else if(move_to_point.ContainsKey(animation)){
            is_idle_flying = false;
            state.queue_and_start(
                time: 120, 
                start_action:()=> move_to_point_begin(move_to_point[animation])
            );
        }
        else{
            is_idle_flying = true;
            state.queue_and_start(time: animator.get_clip_length($"{animation}"),start_action:()=>play_attack_animation(animation));
        }
    }

    private void move_to_player_begin(Transform target){
        move_to_target = target;
            state.queue_and_start(time: 120, 
                start_action:()=>{
                    movement.mod_speed(8f);
                    movement.freeform_move_to(move_to_target, move_to_player_attack);
                }
            );        
    }

    private void move_to_point_begin(Transform target){
        move_to_target = target;
            state.queue_and_start(time: 120, 
                start_action:()=>{
                    movement.mod_speed(8f);
                    movement.freeform_approach_to(move_to_target, move_to_point_attack);
                }
            );        
    }

    // used for when moving towards to to attack.
    // play attack animation, then move back to starting point.
    private void move_to_player_attack(){
        string animation = combat.get_chosen_attack().animation_id;
        state.clear();
        state.queue_and_start(time: animator.get_clip_length(animation), 
            start_action:()=>{
                movement.freeform_move_to(move_to_target);
                play_attack_animation(animation);
            },
            time_out:()=>play_idle_animation()
        );
        state.queue(time: 120, 
            start_action:()=>{
                movement.freeform_move_to(start_point, move_to_attack_finished);
            }
        );
    }

    private void move_to_point_attack(){
        string animation = combat.get_chosen_attack().animation_id;
        state.clear();
        state.queue_and_start(time: animator.get_clip_length(animation), 
            start_action:()=>{
                play_attack_animation(animation);
                move_to_point_camera_adjust();
            },
            time_out:()=>play_idle_animation()
        );
        state.queue(time: 120, 
            start_action:()=>{
                move_to_point_camera_reset();
                movement.freeform_move_to(start_point, move_to_attack_finished);
            }
        );
    }

    // return to idle when reaching starting point.
    private void move_to_attack_finished(){
        movement.reset_speed();
        state.clear();
        combat.attack_end();
    }

    private void play_attack_animation(string animation){
        if(animator.clips.ContainsKey(animation+"Head")){
            animator.Play(animation+"Head");
        }
        if(single_hand_attacks.Contains(animation)){
            Giant2Hand hand = UnityEngine.Random.Range(0,2) == 0? left_hand : right_hand;
            hand.attack(animation+"Hand");
        }
        else if(double_hand_synced_attacks.Contains(animation)){
            left_hand.attack($"{animation}Hand");
            right_hand.attack($"{animation}Hand");
        }
        else if(double_hand_asynced_attacks.Contains(animation)){
            int x = UnityEngine.Random.Range(0,2);
            left_hand.attack($"{animation}{(x==0?1:2)}Hand");
            right_hand.attack($"{animation}{(x==0?2:1)}Hand");
        }
        else
            animator.Play($"{animation}Head");
    }

    private void idle(float time){
        state.queue_and_start(
            time: time,
            start_action: ()=>{
                play_idle_animation(); 
                if(is_idle_flying == false)
                    movement.figure_eight_state(reverse: UnityEngine.Random.Range(0,2)==0, x_factor:.1f, y_factor:.05f);  
            }
        );
    }

    private void play_idle_animation(){
        animator.Rebind();
        animator.CrossFade("Giant2IdleHead",0.1f,0,0);
        //left_hand.animator.Play("Giant2IdleHand");
        //right_hand.animator.Play("Giant2IdleHand");
    }

    public void play_intro_animation(){
        left_hand.animator.Play("Giant2Intro1Hand",0,0);
        right_hand.animator.Play("Giant2Intro2Hand",0,0);
        animator.Play("Giant2IntroHead");
    }

    public void move_to_point_camera_adjust(){
        CameraController.instance.lerp_zoom(size: 12, time: 2);
        CameraController.instance.lerp_regulators(_x_bounds: new Vector2(-10,10), _y_bounds: new Vector2(-0.15f, -0.15f), time: 2);
    }
    public void move_to_point_camera_reset(){
        CameraController.instance.reset_offset(2);
        CameraController.instance.reset_zoom(2);
        CameraController.instance.reset_regulators(2);
    }

    protected override void death_start(){
        StopAllCoroutines();
        stop_all();
        no_state();
        state.clear_and_stop();
        state = null;
        animator.PlayInstant("Giant2DeathHead");
        left_hand.death();
        right_hand.death();
        sprites.play_death_effect(4f);
        enable_body_colliders(0);
        StartCoroutine(sprites.lerp_value("shadow_water", "_amount", .5f, -0.2f, 3f ));
        movement.zero_velocity(); // stop velocity in case the boss is dashing.
        base.death_start();
        StartCoroutine(Util.timer(
            animator.get_clip_length("Giant2DeathHead")+3,
            time_out:()=>{
                // AudioManager.stop_music();
                UiManager.instance.play_enemy_vanquished();
                gameObject.SetActive(false);
                base.death_complete();
            }
        ));
    }

    public void yell_camera_shake() => CameraController.instance.shake_camera(4f,.7f, true);
    public void intro_camera_shake(){
        CameraController.instance.shake_camera(5.5f, 0.55f, true);
    }
    public void intro_water_rush_sound(){
        sound.play_diegetic_loop("water_rushing");
        sound.set_diegetic_instance_parameter("water_rushing", "intensity",1);
    }
    public void play_music(){
        AudioManager.play_music("music_the_giant_2");
    }

    protected void link_events(){
        link_game_manager();
        health.death                            += kill;
        health.death                            += movement.StopAllCoroutines;
        combat.attack_chosen                    += attack;
        combat.attack_ended                     += idle;
        left_hand.attack_ended                  += combat.attack_end;
        right_hand.attack_ended                 += combat.attack_end;
        health.damaged                          += sprites.play_damaged_flash;
        health.damaged                          += left_hand.sprite.play_damaged_flash;
        health.damaged                          += right_hand.sprite.play_damaged_flash;
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
        left_hand.attack_ended                  -= combat.attack_end;
        right_hand.attack_ended                 -= combat.attack_end;
        health.damaged                          -= sprites.play_damaged_flash;
        health.damaged                          -= left_hand.sprite.play_damaged_flash;
        health.damaged                          -= right_hand.sprite.play_damaged_flash;
        unlink_particles();
        unlink_combat();
        unlink_movement();
    }
}
