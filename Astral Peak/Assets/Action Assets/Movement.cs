using UnityEngine;

public class Movement : MonoBehaviour{
    [SerializeField] private bool grounded = false;
    [SerializeField] private float top_speed = 5.0f;
    [SerializeField] private float acceleration = 5.0f;
    [SerializeField, Range(0f, 1f)] private float deceleration = 0.85f;
    [SerializeField] private Vector2 move_direction = new Vector2();
    [SerializeField] private BoxCollider2D ground_check;
    [SerializeField] private LayerMask ground_mask;
    [SerializeField] private Rigidbody2D rb;

    void FixedUpdate(){
        is_grounded();
        horizontal_move();
        vertical_move();
        decelerate();
    }

    public void move_left(bool x){
        move_direction += (x == true)? new Vector2(-1,0) : new Vector2(1,0);
    }

    public void move_right(bool x){
        move_direction += (x == true)? new Vector2(1,0) : new Vector2(-1,0);
    }

    public void move_up(bool x){
        move_direction += (x == true)? new Vector2(0,1) : new Vector2(0,-1);
    }

    private void is_grounded(){
        grounded = Physics2D.OverlapAreaAll(ground_check.bounds.min, ground_check.bounds.max, ground_mask).Length > 0;
    }

    private void horizontal_move(){
        if(Mathf.Abs(move_direction.x) <= 0)
            return;

        // accelerate
        float increment = move_direction.x * acceleration;
        // regulate
        float newSpeed = Mathf.Clamp(rb.velocity.x + increment, -top_speed, top_speed);
        // apply
        rb.velocity = new Vector2(newSpeed, rb.velocity.y);        
    }

    private void vertical_move(){
        if(Mathf.Abs(move_direction.y) > 0 && grounded)
            rb.velocity = new Vector2(rb.velocity.x, move_direction.y * top_speed);
    }

    private void decelerate(){
        // decelerate when grounded and not moving.
        if(grounded && Mathf.Abs(move_direction.x) < 0.1f)
            rb.velocity *= deceleration;        
    }
}
