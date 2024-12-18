using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class TutorialEnemy : Enemy{
    
    
    
    
    
    // Data:
    [SerializeField] AnimationEvent animation_event;
    [SerializeField] HollowParticlesHandler particles;
    [SerializeField] Collider2DFeedback agro_area;
    [SerializeField] HollowAudio sound;
    [SerializeField] Collider2D hurt_box;
    bool target_in_range;
    float stun_state_timer = .5f;
    [SerializeField] float 
        idle_speed, idle_acceleration, idle_deceleration,
        alert_speed, alert_acceleration, alert_deceleration;
    Dictionary<string, Action> animation_events;





    // Base.
    void Awake() => create_animation_events();
    void Start(){
        link_events();
        idle_movement();
        state_switch(pathing_loop());
        target = origin;
    }

    void OnDestroy(){
        unlink_events();
    }





    // States:
    IEnumerator idle(float x){
        animator.Play("idle");
        yield return new WaitForSeconds(x);
        recovery_state();
        yield break;
    }

    protected override IEnumerator follow(){        
        animator.Play(target_in_range? "run" : "walk");
        yield return base.follow();
    }

    public override void kill() => state_switch(death_coroutine());
    protected IEnumerator death_coroutine(){
        animator.Play("death");
        // wait one second for animation to play out.
        yield return new WaitForSeconds(animator.GetCurrentAnimatorClipInfo(0).Length + 3);
        base.kill();
        Destroy(gameObject);
    }

    public void stun_state() =>  state_switch(stun_state_loop());
    protected IEnumerator stun_state_loop(){
        // to prevent the ai from chasing once staggered.
        unlink_combat();
        yield return new WaitForSeconds(stun_state_timer);

        link_combat();
        recovery_state();
        yield break;
    }

    protected IEnumerator alert(){
        animator.Play("yell");
        yield break;
    }

    public void recovery_state() => state_switch(target_in_range == true? follow() : retreat_loop());
        
    void alert_state() => state_switch(alert());

    void player_in_range(Collider2D col){
        target = col.gameObject.transform;
        target_in_range = true;
        alert_movement();
        alert_state();
    }

    void player_left_range(Collider2D col){
        target_in_range = false;
        movement.stop();
        idle_movement();
        recovery_state();
        particles.stop_yell();
        target = null;
    }
    void idle_movement(){
        movement.set_speed(idle_speed); 
        movement.set_deceleration(idle_deceleration);
        movement.set_acceleration(idle_acceleration);        
    }
    void alert_movement(){
        movement.set_speed(alert_speed); 
        movement.set_deceleration(alert_deceleration);
        movement.set_acceleration(alert_acceleration);        
    }

    void pathing_animations(MovementOption _movement){
        switch(_movement){
            case MovementOption.LEFT:
            case MovementOption.RIGHT:
                animator.Play("walk");
                break;
            case MovementOption.NONE:
                animator.Play("idle");
                break;
        }
    }






    // Animation
    private void handle_animation_events(string x) => animation_events[x]();
    private void create_animation_events(){
        animation_events = new Dictionary<string, Action>(){
            // Idle
            {"idle_rattle", () => sound.idle_rattle()},

            // Walk and Run
            {"footstep", () => {
                sound.footstep();
                sound.walk_rattle();
            }},

            // Yell
            {"yell_start", () =>{
                sound.yell();
                particles.play_yell();
                CameraController.instance.shake_camera(2, .55f);
                flip_to_target();
            }},
            {"yell_end", () =>{
                particles.stop_yell();
                flip_to_target();
                recovery_state();
            }},

            // Death
            {"death_start", () =>{
                hurt_box.enabled = false;
                unlink_combat();
                unlink_movement();
                unlink_health();
                particles.stop_yell();
                particles.stop_ambience();
                particles.play_death_ambience();
                sprite.play_death_effect(2.25f);
                sound.death_rattle();
            }},
            {"death_explosion_sound", ()=>{
                sound.death_explosion();
            }},
            {"death_explosion", () =>{
                particles.stop_death_ambience();
                particles.emit_death_explosion();
                CameraController.instance.shake_camera(.55f, .70f);
            }},
        };
    }






    // linkage
    protected void link_events(){
        link_combat();
        link_health();
        link_movement();
        link_animation_events();
    }

    protected void unlink_events(){
        unlink_combat();
        unlink_health(); 
        unlink_movement();
        unlink_animation_events();
    }

    private void link_combat(){
        agro_area.trigger_enter += player_in_range;
        agro_area.trigger_exit  += player_left_range;
    }
    private void unlink_combat(){
        agro_area.trigger_enter -= player_in_range;
        agro_area.trigger_exit  -= player_left_range; 
    }

    private void link_movement(){
        movement.move_direction_changed += face_move_dir;
        pathing_movement                += pathing_animations;
    }

    private void unlink_movement(){
        movement.move_direction_changed -= face_move_dir;
        pathing_movement                -= pathing_animations;
        movement.stop();
    }

    protected void link_health(){
        health.damaged += stun_state;
        health.damaged += sprite.play_damaged_flash;
        health.death += kill;
        health.knockback += movement.knockback;
    }
    protected void unlink_health(){
        health.damaged -= stun_state;
        health.damaged -= sprite.play_damaged_flash;
        health.death -= kill;
        health.knockback -= movement.knockback;
    }

    protected void link_animation_events() => animation_event.signal += handle_animation_events;
    protected void unlink_animation_events() => animation_event.signal -= handle_animation_events;
}
