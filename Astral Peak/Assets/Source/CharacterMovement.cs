using UnityEngine;
using UnityEngine.InputSystem.Utilities;

public class CharacterMovement : Movement{
    [SerializeField] private bool grounded = false;
    [SerializeField] private bool jumping = false;
    [SerializeField] private float jump_time = 1.0f;
    [SerializeField] private float jump_force = 10.0f;

    public override void FixedUpdate(){
        is_grounded();
        horizontal_move();
        vertical_move();
        decelerate();        
    }

    // ground check
    private void is_grounded(){
        grounded = Physics2D.OverlapAreaAll(ground_check.bounds.min, ground_check.bounds.max, ground_mask).Length > 0;
    }

    // jump command
    public void jump(bool x) {jumping = x;}

    // override vertical move to take jumping into account.
    protected override void vertical_move(){
        if(Mathf.Abs(move_direction.y) > 0 && grounded)
            rb.velocity = new Vector2(rb.velocity.x, move_direction.y * top_speed);      
    }

    protected override void decelerate(){
        // decelerate when grounded and not moving.
        if(grounded && Mathf.Abs(move_direction.x) < 0.1f)
            rb.velocity *= deceleration;        
    } 
}