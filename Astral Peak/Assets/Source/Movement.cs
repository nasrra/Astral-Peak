using System;
using System.Collections;
using System.Collections.Generic;
using Deluz;
using UnityEngine;

public class Movement : MonoBehaviour{
    public event Action<Vector2> move_direction_changed;
    public event Action 
        knockedback, knockback_ended,
        dashed, dash_end,
        target_reached;

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





    // getters and setters.
    public void set_speed(float x) => top_speed = x;
    public void set_deceleration(float x) => deceleration = x;
    public void set_acceleration(float x) => acceleration = x;
    public void reset_deceleration() => deceleration = original_deceleration;
    public void is_knockbackable(int x) => can_knockback = x != 0;
    public Vector2 get_move_direction() => move_direction;
    public void update_move_direction(Vector2 direction){
        move_direction += direction;
        move_direction_changed?.Invoke(move_direction);
    }
    public void set_move_direction(Vector2 direction){
        move_direction = direction;
        move_direction_changed?.Invoke(move_direction);        
    }
    public void set_gravity(float _gravity) => rb.gravityScale = _gravity;





    //movment functions
    public void move_left(bool x)   => update_move_direction((x == true)? new Vector2(-1,0) : new Vector2(1,0));
    public void move_right(bool x)  => update_move_direction((x == true)? new Vector2(1,0)  : new Vector2(-1,0));
    public void move_up(bool x)     => update_move_direction((x == true)? new Vector2(0,1)  : new Vector2(0,-1));
    public void move_down(bool x)   => update_move_direction((x == true)? new Vector2(0,-1) : new Vector2(0,1));
    public virtual void clear_move_direction(){
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
    protected virtual void horizontal_move(){    
        if(Mathf.Abs(move_direction.x) <= 0)
            return;
        float increment = move_direction.x * acceleration;
        float newSpeed = Mathf.Clamp(rb.linearVelocity.x + increment, -top_speed, top_speed);
        rb.linearVelocity = new Vector2(newSpeed, rb.linearVelocity.y);     
    }
    protected virtual void vertical_move() => rb.linearVelocity = new Vector2(rb.linearVelocity.x, move_direction.y * top_speed);
    protected virtual void decelerate() => rb.linearVelocity *= deceleration;





    // state machine.
    public void no_state(){
        state_switch(null);
        clear_move_direction();
    }
    private void state_switch(IEnumerator _state){
        if(state != null)
            StopCoroutine(state);
        state = _state!=null? StartCoroutine(_state) : null;
    }
    protected void state_switch_default() => state_switch(default_state());
    protected IEnumerator default_state(){
        while(true){
            move();
            yield return new WaitForFixedUpdate();
        }
    }
    private void move(){
        horizontal_move();
        vertical_move();
        decelerate();
    }


    public void dash(Vector3 direction, float force, float duration){
        if(can_dash == true){
            can_dash = false;
            can_knockback = false; // added here in bug case, so 'can_dash' returns back to true for bosses.
            is_dashing = true;
            state_switch(apply_force_loop(dashed, dash_end, direction, force, duration));
        }      
    }
    private void end_dash(){
        state_switch_default();
        is_dashing = false;
        can_knockback = true; // added here in bug case, so 'can_dash' returns back to true for bosses.
        StartCoroutine(Util.timer(dash_cooldown, time_out:()=>can_dash=true));
    }
    public void knockback(KnockbackData data){
        if(can_knockback == true)
            state_switch(apply_force_loop(knockedback, knockback_ended, (transform.position - data.transform.position + new Vector3(0,2.25f,0)).normalized, data.force, data.duration));
    }
    //TODO: fix this with a timer Coroutine from Util.
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
        clear_move_direction();
        if(transform.rotation.eulerAngles.y == 180)
            move_left(true);
        else
            move_right(true); 
    }
    public void move_to_target_state(Transform transform) => state_switch(move_to_target(transform));
    protected IEnumerator move_to_target(Transform target){
        while(target != null){
            float dist = (transform.position - target.position).x;
            // if we are not moving right, move right.
            if(dist < 0 && get_move_direction() != new Vector2(1,0)){
                clear_move_direction();
                move_right(true);
            }
            // if we are not moving left, move left.
            if(dist > 0 && get_move_direction() != new Vector2(-1,0)){
                clear_move_direction();
                move_left(true);
            }
            if(Mathf.Abs(dist) >= 0.25f)
                target_reached?.Invoke();
            move();
            yield return new WaitForFixedUpdate();
        }
        yield break;
    }
    public void pathing_loop_state(List<MovementPath> paths) => state_switch(pathing_loop(paths));
    public IEnumerator pathing_loop(List<MovementPath> paths){
        MovementPath current_path;
        int path_index = 0;
        while(true){
            // start movement.
            current_path = paths[path_index];
            movement(current_path.movement, true);
            move();
            yield return new WaitForSeconds(current_path.duration);
            clear_move_direction();
            path_index = ((path_index + 1) >= paths.Count)? 0 : path_index + 1;
            yield return new WaitForFixedUpdate();
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

// Path that the Ai will follow.
[System.Serializable]
public struct MovementPath{
    // direction to move in.
    public MovementOption movement;
    // duration of movement.
    public float duration;
}