using UnityEngine;

public class CharacterMovement : Movement{
    [SerializeField] private bool grounded = false;
    [SerializeField] private bool jumping = false;
    [SerializeField] private float jump_time = 1.0f;
    [SerializeField] private float jump_force = 10.0f;
    [SerializeField] private float jump_force_multiplier = 0.1f;
    [SerializeField] private float jump_time_counter = 0.0f;

    public override void FixedUpdate(){
        is_grounded();
        horizontal_move();
        vertical_move();
        decelerate();        
    }

    // ground check
    private void is_grounded(){
        grounded = Physics2D.OverlapAreaAll(ground_check.bounds.min, ground_check.bounds.max, ground_mask).Length > 0;
        if(grounded)
            jump_time_counter = 0.0f;
    }

    // jump command
    public void jump() {
        jumping = true;
        move_direction.y = 1;
    }

    public void end_jump(){
        jumping = false;
        jump_time_counter = jump_time;
        move_direction.y = 0;
    }

    // override vertical move to take jumping into account.
    protected override void vertical_move(){
        // if we want to jump, start jumping.
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

    protected override void decelerate(){
        // decelerate when grounded and not moving.
        if(grounded == true && Mathf.Abs(move_direction.x) < 0.1f)
            rb.velocity *= deceleration;        
    } 
}