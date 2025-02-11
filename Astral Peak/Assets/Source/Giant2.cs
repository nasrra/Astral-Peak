using UnityEngine;
using Entropek;
using System;
using System.Collections.Generic;
using Entropek.Collections;
using AYellowpaper.SerializedCollections;

public class Giant2 : Boss<Movement>{
    Coroutine idle_state;
    [Header("Giant2")]
    [SerializeField] FinalBossRoomGroundHandler ground_handler;
    [SerializeField] SerializedDictionary<string, Transform> move_to_point; // points attacks can move to.
    // which animation layer to be used for each attack.
    private HashSet<string> 
        head_attacks = new(){
            "Giant2YellProjectiles"
        },
        single_hand_attacks = new(){
            "Giant2FistSlam"
        },
        double_hand_attacks = new(){
            "Giant2FingerGun",
            "Giant2HandClap"
        };
    // what behaviour occurs when calling said attack.
    private HashSet<string>
        move_to_player = new(){
            "Giant2FistSlam",
            "Giant2HandClap"
        },
        idle_fly = new(){
            "Giant2YellProjectiles"
        };
    [SerializeField] GameObject left_hand_sludge_audio_player, right_hand_sludge_audio_player;
    [SerializeField] Transform left_hand, right_hand, head, player_hover_point, start_point, move_to_target;
    [SerializeField] char hand = 'L';

    void Awake(){
        sound.set_functions(new GiantSound(gameObject));
        state = new StateQueue(this, fly_and_attack_state);
        link_events();
    }
    void Start(){
        set_phase_data("phase_1");
        //switch_phase();
    }
    void OnDestroy() => unlink_events();

    protected override void entered_game_state(GameState state){
        if(state == GameState.CUTSCENE)
            this.state.clear_and_stop();
        base.entered_game_state(state);
    }

    protected override void exited_game_state(GameState state){
        if(state == GameState.CUTSCENE)
            idle(2);
        base.exited_game_state(state);
    }

    private void fly_and_attack_state(){
        animator.Play("Giant2HeadIdle");
        animator.Play("Giant2HandIdleL");
        animator.Play("Giant2HandIdleR");
        combat.chose_attack_state(target);
    }

    protected override void attack(BossAttack attack){
        string animation = attack.animation_id;
        combat.halt();
        if(move_to_player.Contains(animation))
            state.queue_and_start(
                time: 120, 
                start_action:()=> move_to_player_begin(player_hover_point)
            );
        else if(move_to_point.ContainsKey(animation))
            state.queue_and_start(
                time: 120, 
                start_action:()=> move_to_point_begin(move_to_point[animation])
            );
        else
            state.queue_and_start(time: animator.get_clip_length(animation),start_action:()=>play_attack_animation(animation));
    }

    private void move_to_player_begin(Transform target){
        move_to_target = target;
            state.queue_and_start(time: 120, 
                start_action:()=>{
                    movement.mod_speed(8f);
                    movement.freeform_move_to(move_to_target);
                    movement.target_reached += move_to_player_attack;
                }
            );        
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

    // used for when moving towards to to attack.
    // play attack animation, then move back to starting point.
    private void move_to_player_attack(){
        movement.target_reached -= move_to_player_attack;
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
                movement.freeform_move_to(start_point);
                movement.target_reached += move_to_attack_finished;
            }
        );
    }

    private void move_to_point_attack(){
        movement.target_reached -= move_to_point_attack;
        string animation = combat.get_chosen_attack().animation_id;
        state.clear();
        state.queue_and_start(time: animator.get_clip_length(animation), 
            start_action:()=>{
                play_attack_animation(animation);
                move_to_point_camera_adjust();
                movement.target_reached += move_to_attack_finished;
            },
            time_out:()=>play_idle_animation()
        );
        state.queue(time: 120, 
            start_action:()=>{
                move_to_point_camera_reset();
                movement.freeform_move_to(start_point);
            }
        );
    }

    // return to idle when reaching starting point.
    private void move_to_attack_finished(){
        movement.reset_speed();
        state.clear();
        combat.attack_end();
        movement.target_reached -= move_to_attack_finished;
    }

    // random left or right hand attack.
    private char choose_hand(){
        hand = UnityEngine.Random.Range(0,2) == 0? 'L' : 'R';
        return hand;
    }
    public void play_hands_weapon_flash() => play_weapon_flash(new List<string>(){"left_hand","right_hand"});
    private void play_attack_animation(string animation){
        if(single_hand_attacks.Contains(animation))
            single_hand_attack(animation);
        else if(double_hand_attacks.Contains(animation))
            double_hand_attack(animation);
        else
            head_attack(animation);
    }
    private void single_hand_attack(string animation){
        choose_hand();
        set_hand_audio_player();
        animator.Play(animation + hand);
    }
    private void double_hand_attack(string animation){
        //choose_hand();
        //sound.set_audio_player(hand == 'L'?left_hand.gameObject : right_hand.gameObject);
        animator.Play(animation + 'L');
        animator.Play(animation + 'R');
    }
    private void head_attack(string animation){
        set_head_audio_player();
        animator.Play(animation);
    }

    private void set_head_audio_player() => sound.set_audio_player(head.gameObject);
    private void set_left_hand_audio_player() => sound.set_audio_player(left_hand.gameObject);
    private void set_right_hand_audio_player() => sound.set_audio_player(right_hand.gameObject);
    private void set_hand_audio_player() => sound.set_audio_player(hand == 'L'?left_hand.gameObject : right_hand.gameObject);
    private void set_left_hand_sludge_audio_player() => sound.set_audio_player(left_hand_sludge_audio_player);
    private void set_right_hand_sludge_audio_player() => sound.set_audio_player(right_hand_sludge_audio_player);

    int get_current_ground_piece(){
        int x = -1;
        Collider2D other = Physics2D.OverlapCircle(hand == 'L'?left_hand.position : right_hand.position, .5f, LayersManager.BITWISE_GROUND);
        if(other != null)
            Int32.TryParse(other.name, out x);
        return x;
    }

    public void fist_slam_camera_shake() => CameraController.instance.shake_camera(.8f,.65f,false);
    public void clap_camera_shake() => CameraController.instance.shake_camera(.8f,.5f,false);


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
                play_idle_animation(); 
                movement.figure_eight_state(reverse: false, x_factor:.1f, y_factor:.05f);  
            }
        );
    }

    private void play_idle_animation(){
        animator.Rebind();
        animator.Play("Giant2HeadIdle");
        animator.Play("Giant2HandIdleL");
        animator.Play("Giant2HandIdleR");
    }

    public void play_intro_animation(){
        animator.Play("Giant2HeadIntro");
        animator.Play("Giant2HandIntroL");
        animator.Play("Giant2HandIntroR");
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
        animator.Play("Giant2HeadDeath");
        animator.Play("Giant2HandDeathL");
        animator.Play("Giant2HandDeathR");
        no_state();
        state.clear_and_stop();
        StartCoroutine(Util.timer(
            animator.get_clip_length("Giant2HeadDeath")+3,
            start_action:()=>{
                if(idle_state != null)
                    StopCoroutine(idle_state);
                enable_body_colliders(0);
                stop_all();
                sprites.play_death_effect(4f);
                movement.zero_velocity(); // stop velocity in case the boss is dashing.
                particles.stop_all_particles();
                base.death_start();
            },
            time_out:()=>{
                AudioManager.stop_music();
                UiManager.instance.play_enemy_vanquished();
                gameObject.SetActive(false);
                base.death_complete();
            }
        ));
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
