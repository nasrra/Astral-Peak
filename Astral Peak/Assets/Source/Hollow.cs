using System;
using Deluz;
using UnityEngine;

public class Hollow : Enemy{
    // Data:
    public event Action on_start;
    [Header("Hollow")]
    [SerializeField] Collider2DFeedback agro_area;
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
        unlink_events();
        animator.Play("HollowDeath",0,0);
        StartCoroutine(Util.timer(
            animator.get_clip_length("HollowDeath") + 3,
            start_action: ()=>{
                movement.no_state();
                enable_body_colliders(0);
                particles.stop_all_particles();
                sprites.play_death_effect(1.5f);
                if(stun_state != null)
                    StopCoroutine(stun_state);
            },
            time_out: ()=>{
                base.kill();
                Destroy(gameObject);
            }
        ));
    }

    public void stun() => 
        state_switch(ref stun_state, Util.timer(
            stun_state_timer,
            start_action:()=>{
                animator.Play("HollowIdle");
                unlink_combat();        
                set_body_colliders_exclude_layers(~LayersManager.BITWISE_GROUND);
            },
            time_out:()=>{
                link_combat();
                recovery_state();
                set_body_colliders_exclude_layers(~(LayersManager.BITWISE_GROUND | LayersManager.BITWISE_PLAYER | LayersManager.BITWISE_ENEMY | LayersManager.BITWISE_HURTBOX));
            }
        ));

    public void recovery_state(){
        particles.stop_particle("yell");
        if(alerted == false)
            alert();
        else
            movement.move_to_target_state(target);
    }
        
    public void summon_state(){
        animator.Play("HollowSummon");
        play_summoning_animation();
        movement.no_state();
    }

    public void alert(){
        movement.no_state();
        animator.Play("HollowYell");
        alerted = true;
        alert_movement();
    }

    // used in animator for alert animation.
    public void move_to_target() => movement.move_to_target_state(target);

    void player_in_range(Collider2D col){
        target = Player.instance.transform;
        alert();
    }

    void player_left_range(Collider2D col){
        movement.no_state();
        idle_movement();
        particles.stop_particle("yell");
        target = origin;
        alerted = false;
        movement.move_to_target_state(target);
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

    void move_direction_changed(Vector2 move_direction){
        if(move_direction != Vector2.left && move_direction != Vector2.right)
            animator.Play("HollowIdle");
        else
            animator.Play(alerted == true? "HollowRun" : "HollowWalk", 0, 0); // force the animation to play (0,0);
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
        movement.target_reached += target_reached;
        movement.move_direction_changed += face_direction;
        movement.move_direction_changed += move_direction_changed;
    }

    private void unlink_movement(){
        movement.target_reached -= target_reached;
        movement.move_direction_changed -= face_direction;
        movement.move_direction_changed -= move_direction_changed;
    }

    protected void link_health(){
        health.damaged += stun;
        health.damaged += sprites.play_damaged_flash;
        health.death += kill;
        health.knockback += movement.knockback;
    }
    protected void unlink_health(){
        health.damaged -= stun;
        health.damaged -= sprites.play_damaged_flash;
        health.death -= kill;
        health.knockback -= movement.knockback;
    }
}
