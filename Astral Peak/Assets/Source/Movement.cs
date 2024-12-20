using System;
using System.Collections;
using DocumentFormat.OpenXml.Wordprocessing;
using UnityEngine;

public class Movement : MonoBehaviour{
    public event Action 
        move_direction_changed, 
        knockedback, knockback_ended,
        dashed, dash_end;

    [Header("Movement")]
    [SerializeField] protected bool 
        can_knockback   = true,
        can_dash        = true,
        is_dashing      = false;
    [SerializeField] protected float top_speed = 5.0f;
    [SerializeField] protected float acceleration = 5.0f;
    [SerializeField] protected float dash_cooldown = 1.0f;
    [SerializeField, Range(0f, 1f)] protected float deceleration = 0.85f;
    [SerializeField] protected Vector2 move_direction = new Vector2();
    [SerializeField] protected Rigidbody2D rb;
    private float original_gravity, original_deceleration;
    protected Coroutine state;

    void OnEnable(){
        state_switch_default();
        original_gravity = rb.gravityScale;
        original_deceleration = deceleration;
    }
    void Start() => link();
    void OnDestroy() => unlink();

    public void set_speed(float x) => top_speed = x;
    public void set_deceleration(float x) => deceleration = x;
    public void set_acceleration(float x) => acceleration = x;
    public void reset_deceleration() => deceleration = original_deceleration;
    public void is_knockbackable(int x) => can_knockback = x != 0;
    public Vector2 get_move_direction() => move_direction;

    // update movement direction and fire an event to notify listeners that we have changed.
    private void update_move_direction(Vector2 direction){
        move_direction += direction;
        move_direction_changed?.Invoke();
    }

    public void move_left(bool x)   => update_move_direction((x == true)? new Vector2(-1,0) : new Vector2(1,0));
    public void move_right(bool x)  => update_move_direction((x == true)? new Vector2(1,0)  : new Vector2(-1,0));
    public void move_up(bool x)     => update_move_direction((x == true)? new Vector2(0,1)  : new Vector2(0,-1));
    public void move_down(bool x)   => update_move_direction((x == true)? new Vector2(0,-1) : new Vector2(0,1));
    public virtual void stop(){
        if(rb != null)
            rb.linearVelocityX = 0;
        move_direction = new Vector2(0,0);
    }

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
            case MovementOption.NONE:
                break;
            default:
                throw new SystemException("("+gameObject.name+": " +option+ ") is exclusively a character movement function.");
        }
    }

    protected void switch_state(IEnumerator state){
        if(this.state != null)
            StopCoroutine(this.state);
        this.state = StartCoroutine(state);
    }

    protected void state_switch_default() => switch_state(default_state());
    protected IEnumerator default_state(){
        while(true){
            horizontal_move();
            vertical_move();
            decelerate();
            yield return new WaitForFixedUpdate();
        }
    }

    protected virtual void horizontal_move(){    
        if(Mathf.Abs(move_direction.x) <= 0)
            return;
        // accelerate
        float increment = move_direction.x * acceleration;
        // regulate
        float newSpeed = Mathf.Clamp(rb.linearVelocity.x + increment, -top_speed, top_speed);
        // apply
        rb.linearVelocity = new Vector2(newSpeed, rb.linearVelocity.y);     
    }

    // this causes a bug with the ai path finding, as its x velocity keeps going when it moves
    protected virtual void vertical_move(){
        // if we are currently being knocked back, dont do anything.
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, move_direction.y * top_speed);
    } 

    protected virtual void decelerate(){
        rb.linearVelocity *= deceleration;
    }

    public void dash(Vector3 direction, float force, float duration){
        if(can_dash == true){
            can_dash = false;
            can_knockback = false; // added here in bug case, so 'can_dash' returns back to true for bosses.
            is_dashing = true;
            switch_state(apply_force_loop(dashed, dash_end, direction, force, duration));
        }      
    }

    private void end_dash(){
        state_switch_default();
        is_dashing = false;
        can_knockback = true; // added here in bug case, so 'can_dash' returns back to true for bosses.
        StartCoroutine(dash_cooldown_loop());
    }
    IEnumerator dash_cooldown_loop(){
        yield return new WaitForSeconds(dash_cooldown);
        can_dash = true;
        yield break;
    }

    public void knockback(KnockbackData data){
        if(can_knockback == true)
            switch_state(apply_force_loop(knockedback, knockback_ended, (transform.position - data.transform.position + new Vector3(0,2.25f,0)).normalized, data.force, data.duration));
    }

    protected IEnumerator apply_force_loop(Action start, Action end, Vector3 direction, float force, float t){
        rb.gravityScale = 0;
        // Normalize the final knockback direction
        direction.Normalize();
        // multiply by knock back force.
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(direction * force, ForceMode2D.Impulse);
        start?.Invoke();
        yield return new WaitForSeconds(t);
        rb.gravityScale = original_gravity;
        rb.linearVelocity = Vector2.zero;
        end?.Invoke();
        yield break;
    }

    public void zero_velocity() => rb.linearVelocity = Vector3.zero;

    public void move_in_faced_direction(){
        if(transform.rotation.y == 1){
            Debug.Log("l");
            move_left(true);
        }   
        else{
            Debug.Log("r");
            move_right(true); 
        }
    }

    protected virtual void link(){
        dash_end += end_dash;
        knockback_ended += state_switch_default;
    }
    protected virtual void unlink(){
        dash_end -= end_dash;
        knockback_ended -= state_switch_default;
    }
}

public enum MovementOption{
    UP,   
    DOWN,
    LEFT, 
    RIGHT,
    START_JUMP,
    STOP_JUMP,
    NONE,
}
