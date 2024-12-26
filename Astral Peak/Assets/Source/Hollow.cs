using System;
using System.Collections;
using Deluz;
using UnityEngine;

public class Hollow : Enemy{
    // Data:
    public event Action on_start;
    [Header("Hollow")]
    [SerializeField] Collider2DFeedback agro_area;
    [SerializeField] Collider2D hurt_box;
    bool alerted;
    float stun_state_timer = .5f;
    [SerializeField] float 
        idle_speed, idle_acceleration, idle_deceleration,
        alert_speed, alert_acceleration, alert_deceleration;
    Coroutine stun_state;





    // Base.
    void Awake(){
        sound.set_functions(new HollowSound(sound));
        link_events();
        idle_movement();
        if(target == null)
            target = origin;    
    }
    void Start(){
        movement.pathing_loop_state(paths);
        on_start?.Invoke();
    }

    void OnDestroy(){
        on_start = null;
        unlink_events();
    }





    // States:
    public override void kill(){
        animator.Play("death");
        StartCoroutine(Util.timer(
            animator.GetCurrentAnimatorClipInfo(0).Length + 3,
            start_action: ()=>{
                movement.no_state();
                enable_body_colliders(0);
                particles.stop_all_particles();
                sprite.play_death_effect(1.25f);
                unlink_health();
                hurt_box.enabled = false;
            },
            time_out: ()=>{
                base.kill();
                unlink_events();
                Destroy(gameObject);
            }
        ));
    }

    public void stun() => 
        state_switch(ref stun_state, Util.timer(
            stun_state_timer,
            start_action:()=>{
                unlink_combat();        
            },
            time_out:()=>{
                link_combat();
                recovery_state();
            }
        ));

    public void recovery_state(){
        particles.stop_particle("yell");
        if(alerted == false)
            alert();
        else
            movement.move_to_target_state(target);
        //state_switch(target == origin?
        //    retreat_loop(): 
        //    alerted == true? follow_only() : alert()
        //);
    }
        
    public void summon_state(){
        animator.Play("summon");
        play_summoning_animation();
        movement.no_state();
        //combat.no_state();
    }

    public void alert(){
        animator.Play("yell");
        alerted = true;
        alert_movement();
    }

    void player_in_range(Collider2D col){
        target = Player.instance.transform;
        alert();
    }

    void player_left_range(Collider2D col){
        movement.clear_move_direction();
        idle_movement();
        recovery_state();
        particles.stop_particle("yell");
        target = origin;
        alerted = false;
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

    void move_direction_changed(Vector2 move_direction){
        if(move_direction == Vector2.left || move_direction == Vector2.right)
            animator.Play(alerted==true?"run":"walk");
        else
            animator.Play("idle");
    }

    void target_reached(){
        if(target == origin)
            movement.pathing_loop_state(paths);
    }

    // linkage
    protected void link_events(){
        link_combat();
        link_health();
        link_movement();
    }

    protected void unlink_events(){
        unlink_combat();
        unlink_health(); 
        unlink_movement();
    }

    private void link_combat(){
        if(agro_area == null)
            return;
        agro_area.trigger_enter += player_in_range;
        agro_area.trigger_exit  += player_left_range;
    }
    private void unlink_combat(){
        if(agro_area == null)
            return;
        agro_area.trigger_enter -= player_in_range;
        agro_area.trigger_exit  -= player_left_range; 
    }

    private void link_movement(){
        movement.move_direction_changed += move_direction_changed;
        movement.move_direction_changed += face_direction;
        movement.target_reached += target_reached;
    }

    private void unlink_movement(){
        movement.move_direction_changed -= move_direction_changed;
        movement.move_direction_changed -= face_direction;
        movement.target_reached -= target_reached;
        movement.clear_move_direction();
    }

    protected void link_health(){
        health.damaged += stun;
        health.damaged += sprite.play_damaged_flash;
        health.death += kill;
        health.knockback += movement.knockback;
    }
    protected void unlink_health(){
        health.damaged -= stun;
        health.damaged -= sprite.play_damaged_flash;
        health.death -= kill;
        health.knockback -= movement.knockback;
    }
}
