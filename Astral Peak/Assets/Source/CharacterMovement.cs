using System;
using System.Collections;
using UnityEngine;

public class CharacterMovement : Movement{
    public event Action 
        now_grounded, not_grounded, jumped, 
        dashed, dash_end;

    [Header("Character Movement")]
    [SerializeField] private bool grounded = false;
    [SerializeField] private bool jumping = false;
    [SerializeField] private bool can_jump = true;
    [SerializeField] private bool can_dash = true;
    [SerializeField] private float 
        jump_time, jump_force, jump_force_multiplier, jump_time_counter, 
        dash_time, dash_force, dash_cooldown;
    [SerializeField] private Collider2DFeedback ground_checker;

    void Start() => link();
    void OnDestroy() => unlink();

    // ground check
    private void is_grounded(Collider2D other){
        grounded = true;    
        jump_time_counter = 0.0f; 
        now_grounded?.Invoke();
    }

    private void is_not_grounded(Collider2D other){
        grounded = false;
        not_grounded?.Invoke();
        reset_deceleration();
    }

    // jump command
    public void jump() {
        jumping = true;
        if(grounded == true && can_jump == true){
            set_deceleration(1);
            move_direction.y = 1;
            jumped?.Invoke();
        }
    }

    public void end_jump(){
        jumping = false;
        move_direction.y = 0;
        // ending a jump removes the ability to jump again.
        // Note:
        // 'if check' is to remove a bug where jumping when grounded locks the player out of jumping.
        // the jumptime counter would be greater than jump_time when grounded end_jump();
        // so it should be reset to zero instead, like when grounded.
        if(grounded == false)
            jump_time_counter = jump_time;
        else
            jump_time_counter = 0.0f;
    }

    // override vertical move to take jumping into account.
    protected override void vertical_move(){
        // if we are ggrounded and want to jump, start jumping.
        if(Mathf.Abs(move_direction.y) > 0.1f){
            // if the jump has not exceeded its max height, keeping apply force.
            if(jump_time_counter < jump_time){
                rb.velocity = new Vector2(
                    rb.velocity.x, 
                    move_direction.y * jump_force + (jump_time_counter * jump_force_multiplier) // adding multipler for 'feel'.
                );
                jump_time_counter += Time.deltaTime;
            }
            // if player is still holding jump, do it again.
            else if (jumping == true)
                jump();
            // if not, end the jump.
            else
                end_jump();
        }      
    }

    // used for ai path finding and other state machines.
    public override void movement(MovementOption option, bool flag){
        switch(option){
            case MovementOption.START_JUMP:
                jump();
                return;
            case MovementOption.STOP_JUMP:
                end_jump();
                return;
        }
        base.movement(option, flag);
    }

    public void dash(Vector3 direction, float force, float duration){
        if(can_dash == true){
            can_dash = false;
            can_knockback = false; // added here in bug case, so 'can_dash' returns back to true for bosses.
            switch_state(apply_force_loop(dashed, dash_end, direction, force, duration));
        }      
    }

    private void end_dash(){
        state_switch_default();
        StartCoroutine(dash_cooldown_loop());
        can_knockback = true; // added here in bug case, so 'can_dash' returns back to true for bosses.
    }
    IEnumerator dash_cooldown_loop(){
        yield return new WaitForSeconds(dash_cooldown);
        can_dash = true;
        yield break;
    }
    
    protected override void decelerate(){
        // decelerate when grounded and not moving.
        if(grounded == true && Mathf.Abs(move_direction.x) < 0.1f)
            rb.velocity *= deceleration;        
    } 

    public void is_jumpable(int x) => can_jump = x != 0;

    protected override void link(){
        base.link();
        ground_checker.trigger_enter += is_grounded;
        ground_checker.trigger_exit += is_not_grounded;
        dash_end += end_dash;
    }

    protected override void unlink(){
        base.unlink();
        ground_checker.trigger_enter -= is_grounded;
        ground_checker.trigger_exit -= is_not_grounded;
        dash_end -= end_dash;
    }
}