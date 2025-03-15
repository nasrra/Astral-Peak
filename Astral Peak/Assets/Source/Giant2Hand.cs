using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using Entropek;
using Entropek.Collections;
using UnityEngine;

public class Giant2Hand : MonoBehaviour{
    public event Action attack_ended;
    [SerializeField] SerializedDictionary<string, Transform> move_to_point = new SerializedDictionary<string, Transform>();
    [SerializeField] List<Collider2D> colliders = new List<Collider2D>();
    [field: SerializeField] public AnimatorOverride animator {get; private set;}
    [field: SerializeField] public BossSpriteHandler sprite {get; private set;}
    [field: SerializeField] public ParticleHandler particles {get; private set;}
    [SerializeField] FinalBossRoomGroundHandler ground_handler;
    [SerializeField] SludgeBeam finger_beam;
    [SerializeField] Movement movement;
    [SerializeField] Transform head_follower;
    [SerializeField] Transform head_anchor;
    [SerializeField] Transform ground_checker;
    [SerializeField] AudioPlayer audio_player;
    Transform move_to_target;
    private StateQueue state;
    string current_animation;

    void Awake(){
        state = new StateQueue(this, idle);
        state.start();
        link();
    }

    void OnDestroy(){
        unlink();
    }

    public void idle(){
        animator.CrossFade("Giant2IdleHand",0.1f,0,0);
        movement.freeform_approach_to(head_anchor, ()=>hook_to_parent(head_anchor));
    }

    public void attack(string animation){
        current_animation = animation;
        if(move_to_point.ContainsKey(animation))
            state.queue_and_start(
                time: 120, 
                start_action:()=> move_to_point_begin(move_to_point[animation])
            );
        else{
            state.queue_and_start(
                time: animator.get_clip_length(animation),
                start_action:()=>{animator.Play(animation);}
                //time_out:()=>{attack_ended?.Invoke();}
            );
        }
    }

    private void move_to_point_begin(Transform target){
        // unhook from head socket and approach to desired position.
        unhook_from_parent();
        move_to_target = target;
            state.queue_and_start(time: 120, 
                start_action:()=>{
                    movement.freeform_approach_to(move_to_target, move_to_point_attack);
                }
            );        
    }

    private void move_to_point_attack(){
        // hook into the desired point.
        state.clear();
        hook_to_parent(move_to_target);
        state.queue_and_start(time: animator.get_clip_length(current_animation), 
            start_action:()=>{
                animator.Play(current_animation);
            },
            time_out:()=>{
                attack_ended?.Invoke();
                animator.Play("Giant2IdleHand");
            }
        );

        // unhook from point and return to hand socket on head.
        state.queue(time: 120, 
            start_action:()=>{
                unhook_from_parent();
                movement.freeform_move_to(head_anchor,()=>{
                    state.clear_and_stop();
                    hook_to_parent(head_anchor);
                });
            }
        );
    }

    public void hook_to_parent(Transform _parent){
        movement.rb.bodyType = RigidbodyType2D.Kinematic; 
        transform.parent = _parent;
        movement.halt();
    }

    public void unhook_from_parent(){
        movement.rb.bodyType = RigidbodyType2D.Dynamic; 
        transform.parent = null;
        movement.halt();
    }

    public void turn_on_finger_beam(){
        finger_beam.turn_on();
        finger_beam.set_sound_intensity(1);
    }
    public void turn_off_finger_beam(){
        finger_beam.turn_off();
    }

    int get_current_ground_piece(){
        int x = -1;
        // cahnge to raycast 2d down.
        Collider2D other = Physics2D.OverlapCircle(ground_checker.position, .5f, LayersManager.BITWISE_GROUND);
        if(other != null)
            Int32.TryParse(other.name, out x);
        return x;
    }

    public void fist_slam_camera_shake()    => CameraController.instance.shake_camera(.8f,.65f,false);
    public void clap_camera_shake()         => CameraController.instance.shake_camera(.8f,.5f,false);

    public void use_geysers() => ground_handler.use_geysers(); 

    public void fist_slam_ground_wave(){
        // left and right
        int ground_piece = get_current_ground_piece();
        ground_handler.start_wave(ground_piece + -1, true, .15f, 400f);
        ground_handler.start_wave(ground_piece + 1, false, .15f, 400f);
    }

    public void death(){
        state.clear_and_stop();
        StopAllCoroutines();
        turn_off_finger_beam();
        particles.stop_all_particles();
        sprite.renew();
        animator.PlayInstant("Giant2DeathHand");
        sprite.play_death_effect(4);
    }

    protected void entered_game_state(GameState state){
        if(state == GameState.CUTSCENE)
            this.state.clear_and_stop();
    }

    protected void exited_game_state(GameState state){
        if(state == GameState.CUTSCENE)
            this.state.start();
    }

    protected void play_weapon_flash(){
        sprite.play_charged_flash();
        audio_player.play_non_diegetic_one_shot("boss_weapon_flash");
    }

    public void enable_colliders(int _enabled){
        // Log.MethodCall();
        bool enabled = _enabled == 1? true :false;
        foreach(Collider2D collider in colliders)
            collider.enabled = enabled;
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
