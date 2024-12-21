using System;
using System.Collections;
using UnityEngine;

public class Hollow : Enemy{
    
    
    
    
    
    // Data:
    public event Action on_start;
    [Header("Hollow")]
    [SerializeField] Collider2DFeedback agro_area;
    [SerializeField] Collider2D hurt_box;
    bool target_in_range;
    float stun_state_timer = .5f;
    [SerializeField] float 
        idle_speed, idle_acceleration, idle_deceleration,
        alert_speed, alert_acceleration, alert_deceleration;





    // Base.
    void Awake(){
        sound.set_functions(new HollowSound(sound));
        link_events();
        idle_movement();
        if(state == null)
            state_switch(pathing_loop());
        if(target == null)
            target = origin;    
    }
    void Start(){
        on_start?.Invoke();
    }

    void OnDestroy(){
        on_start = null;
        unlink_events();
    }





    // States:
    IEnumerator idle(float x){
        animator.Play("idle");
        yield return new WaitForSeconds(x);
        recovery_state();
        yield break;
    }

    protected override IEnumerator follow_only(){        
        animator.Play(target_in_range? "run" : "walk");
        yield return base.follow_only();
    }

    public override void kill() => state_switch(death_coroutine());
    protected IEnumerator death_coroutine(){
        enable_body_colliders(0);
        particles.stop_all_particles();
        animator.Play("death");
        sprite.play_death_effect(2.25f);
        unlink_health();
        hurt_box.enabled = false;
        // wait one second for animation to play out.
        yield return new WaitForSeconds(animator.GetCurrentAnimatorClipInfo(0).Length + 3);
        base.kill();
        unlink_events();
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
        target_in_range = true; // keep here for mage boss fight.
        alert_movement();
        yield break;
    }

    public void recovery_state(){
        particles.stop_particle("yell");
        state_switch(target_in_range == true? follow_only() : retreat_loop());   
    }
        
    public void alert_state() => state_switch(alert());

    void player_in_range(Collider2D col){
        target = Player.instance.transform;
        target_in_range = true;
        alert_state();
    }

    void player_left_range(Collider2D col){
        target_in_range = false;
        movement.stop();
        idle_movement();
        recovery_state();
        particles.stop_particle("yell");
        target = origin;
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
}
