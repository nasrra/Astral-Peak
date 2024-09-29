using System;
using UnityEngine;

public class Movement : MonoBehaviour{
    [Header("Movement")]
    [SerializeField] protected float top_speed = 5.0f;
    [SerializeField] protected float acceleration = 5.0f;
    [SerializeField, Range(0f, 1f)] protected float deceleration = 0.85f;
    [SerializeField] protected Vector2 move_direction = new Vector2();
    [SerializeField] protected Rigidbody2D rb;

    public virtual void FixedUpdate(){
        horizontal_move();
        vertical_move();
        decelerate();
    }

    public void move_left(bool x)   => move_direction += (x == true)? new Vector2(-1,0) : new Vector2(1,0);
    public void move_right(bool x)  => move_direction += (x == true)? new Vector2(1,0)  : new Vector2(-1,0);
    public void move_up(bool x)     => move_direction += (x == true)? new Vector2(0,1)  : new Vector2(0,-1);
    public void move_down(bool x)   => move_direction += (x == true)? new Vector2(0,-1) : new Vector2(0,1);
    public void stop()              => move_direction = new Vector2(0,0);

    // used for ai path finding and other state machines. 
    public virtual void movement(MovementOption option, bool flag){
        switch(option){
            case MovementOption.UP:
                move_up(flag);
                break;
            case MovementOption.LEFT:
                move_left(flag);
                break;
            case MovementOption.RIGHT:
                move_right(flag);
                break;
            case MovementOption.DOWN:
                move_down(flag);
                break;
            default:
                throw new SystemException("("+gameObject.name+": " +option+ ") is exclusively a character movement function.");
        }
    }

    public Vector2 get_move_direction() => move_direction;

    protected virtual void horizontal_move(){
        if(Mathf.Abs(move_direction.x) <= 0)
            return;
        // accelerate
        float increment = move_direction.x * acceleration;
        // regulate
        float newSpeed = Mathf.Clamp(rb.velocity.x + increment, -top_speed, top_speed);
        // apply
        rb.velocity = new Vector2(newSpeed, rb.velocity.y);        
    }

    // this causes a bug with the ai path finding, as its x velocity keeps going when it moves
    protected virtual void vertical_move() => rb.velocity = new Vector2(rb.velocity.x, move_direction.y * top_speed);
    protected virtual void decelerate() => rb.velocity *= deceleration;
}

public enum MovementOption{
    UP,   
    DOWN,
    LEFT, 
    RIGHT,
    START_JUMP,
    STOP_JUMP
}
