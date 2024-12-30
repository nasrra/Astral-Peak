using System;
using System.Collections;
using System.Collections.Generic;
using Deluz;
using Unity.VisualScripting;
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
    protected Coroutine move_state, controller_state, dash_state;

    void OnEnable(){
        move_only_state();
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





    // state machine.
    public void no_state(){
        clear_move_direction();
        state_switch(ref move_state, null);
        state_switch(ref controller_state, null);
    }
    private void state_switch(ref Coroutine state, IEnumerator _state){
        if(state != null)
            StopCoroutine(state);
        state = _state != null ? StartCoroutine(_state) : null;
    }
    public void zero_velocity() => rb.linearVelocity = Vector3.zero;




    //movment functions
    public void move_left(bool x)   => update_move_direction((x == true)? new Vector2(-1,0) : new Vector2(1,0));
    public void move_right(bool x)  => update_move_direction((x == true)? new Vector2(1,0)  : new Vector2(-1,0));
    public void move_up(bool x)     => update_move_direction((x == true)? new Vector2(0,1)  : new Vector2(0,-1));
    public void move_down(bool x)   => update_move_direction((x == true)? new Vector2(0,-1) : new Vector2(0,1));
    public virtual void clear_move_direction(){
        if(rb != null)
            rb.linearVelocityX = 0;
        //move_direction = new Vector2(0,0);
        set_move_direction(Vector2.zero);
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
                clear_move_direction();
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

    public void move_only_state(){
        state_switch(ref move_state, move());
        state_switch(ref controller_state, null);
    }
    private IEnumerator move(){
        while(true){
            horizontal_move();
            vertical_move();
            decelerate();
            yield return new WaitForFixedUpdate();
        }
    }
    public void dash(Vector3 direction, float force, float duration){
        if(can_dash == true){
            can_dash = false;
            can_knockback = false; // added here in bug case, so 'can_dash' returns back to true for bosses.
            is_dashing = true;
            state_switch(ref move_state, apply_force_loop(dashed, dash_end, direction, force, duration));
            state_switch(ref controller_state, null);

        }      
    }
    private void end_dash(){
        move_only_state();
        is_dashing = false;
        can_knockback = true; // added here in bug case, so 'can_dash' returns back to true for bosses.
        StartCoroutine(Util.timer(dash_cooldown, time_out:()=>can_dash=true));
    }
    public void knockback(KnockbackData data){
        if(can_knockback == true){
            state_switch(ref move_state, apply_force_loop(knockedback, knockback_ended, (transform.position - data.transform.position + new Vector3(0,2.25f,0)).normalized, data.force, data.duration));
            state_switch(ref controller_state, null);
        }
    }
    protected IEnumerator apply_force_loop(Action start, Action end, Vector3 direction, float force, float time) 
        => Util.timer(
            time,
            start_action:()=>{
                rb.gravityScale = 0;
                // Normalize the final knockback direction
                direction.Normalize();
                // multiply by knock back force.
                rb.linearVelocity = Vector2.zero;
                rb.AddForce(direction * force, ForceMode2D.Impulse);
                start?.Invoke();
            },
            time_out:()=>{
                rb.gravityScale = original_gravity;
                rb.linearVelocity = Vector2.zero;
                end?.Invoke();
            }
        );

    public void move_to_target_state(Transform target) {
        clear_move_direction();
        state_switch(ref move_state, move());
        state_switch(ref controller_state, move_to_target(target));
    }
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
            if(Mathf.Abs(dist) <= 0.1f)
                target_reached?.Invoke();
            yield return new WaitForFixedUpdate();
        }
        yield break;
    }
    public void pathing_loop_state(List<MovementPath> paths){
        clear_move_direction();
        state_switch(ref move_state, move());
        state_switch(ref controller_state, pathing_loop(paths));    
    }
    public IEnumerator pathing_loop(List<MovementPath> paths){
        MovementPath current_path;
        int path_index = 0;
        while(true){
            // start movement.
            current_path = paths[path_index];
            movement(current_path.movement, true);
            yield return new WaitForSeconds(current_path.duration);
            clear_move_direction();
            path_index = ((path_index + 1) >= paths.Count)? 0 : path_index + 1;
            yield return new WaitForFixedUpdate();
        }
    }
    public void figure_eight_state(bool reverse = false){
        state_switch(ref move_state, move());
        state_switch(ref controller_state, figure_eight(reverse));
    }
    protected IEnumerator figure_eight(bool reverse = false){
        float elapsed_time = 0;
        float reverse_factor = reverse==false? 1 : -1;
        float y_speed_factor = top_speed*10;
        float x_speed_factor = y_speed_factor/2;
        while(true){
            elapsed_time += Time.deltaTime;
            float sin_x = Mathf.Sin(elapsed_time * Time.deltaTime * x_speed_factor * reverse_factor);
            float sin_y = Mathf.Sin(elapsed_time * Time.deltaTime * y_speed_factor * reverse_factor);
            set_move_direction(new Vector2(sin_x, sin_y));
            yield return new WaitForFixedUpdate();
        }
    }

    protected virtual void link(){
        dash_end += end_dash;
        knockback_ended += move_only_state;
    }
    protected virtual void unlink(){
        dash_end -= end_dash;
        knockback_ended -= move_only_state;
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