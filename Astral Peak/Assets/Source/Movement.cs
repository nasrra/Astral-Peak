using System;
using System.Collections;
using UnityEngine;

public class Movement : MonoBehaviour{
    [Header("Movement")]
    [SerializeField] protected bool knockedback = false;
    [SerializeField] protected float top_speed = 5.0f;
    [SerializeField] protected float acceleration = 5.0f;
    [SerializeField, Range(0f, 1f)] protected float deceleration = 0.85f;
    [SerializeField] protected Vector2 move_direction = new Vector2();
    [SerializeField] protected Rigidbody2D rb;
    
    public event Action<Vector2> move_direction_changed;
    private Coroutine knockback_coroutine;

    public virtual void FixedUpdate(){
        horizontal_move();
        vertical_move();
        decelerate();
    }

    // update movement direction and fire an event to notify listeners that we have changed.
    private void update_move_direction(Vector2 direction){
        move_direction += direction;
        move_direction_changed?.Invoke(move_direction);
    }
    public void move_left(bool x)   => update_move_direction((x == true)? new Vector2(-1,0) : new Vector2(1,0));
    public void move_right(bool x)  => update_move_direction((x == true)? new Vector2(1,0)  : new Vector2(-1,0));
    public void move_up(bool x)     => update_move_direction((x == true)? new Vector2(0,1)  : new Vector2(0,-1));
    public void move_down(bool x)   => update_move_direction((x == true)? new Vector2(0,-1) : new Vector2(0,1));
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
        // if there is no input or we are currently being knocked back, return.
        if(Mathf.Abs(move_direction.x) <= 0 || knockedback == true)
            return;

        // accelerate
        float increment = move_direction.x * acceleration;
        // regulate
        float newSpeed = Mathf.Clamp(rb.velocity.x + increment, -top_speed, top_speed);
        // apply
        rb.velocity = new Vector2(newSpeed, rb.velocity.y);        
    }

    // this causes a bug with the ai path finding, as its x velocity keeps going when it moves
    protected virtual void vertical_move(){
        // if we are currently being knocked back, dont do anything.
        if(knockedback == true)
            return;
        rb.velocity = new Vector2(rb.velocity.x, move_direction.y * top_speed);
    } 
    protected virtual void decelerate(){
        if(knockedback == true)
            rb.velocity *= deceleration;
    }

    public void knockback(Vector3 direction, float force, float duration) => StartCoroutine(knockback_loop(direction, force, duration));

    IEnumerator knockback_loop(Vector3 direction, float force, float t){
        rb.gravityScale = 0;
        knockedback = true;

        // timer to count down from.
        float knockback_timer = t;
        // Add upward force to the knockback direction
        direction += Vector3.up * 0.55f;
        // Normalize the final knockback direction
        direction.Normalize();
        // multiply by knock back force.
        rb.velocity = Vector2.zero;
        rb.AddForce(direction * force, ForceMode2D.Impulse);
        


        while(knockback_timer >= 0.0f){
            knockback_timer -= Time.deltaTime;
            yield return null; 
        }

        rb.gravityScale = 2;
        knockedback = false;
        yield break;
    }
}

public enum MovementOption{
    UP,   
    DOWN,
    LEFT, 
    RIGHT,
    START_JUMP,
    STOP_JUMP
}
