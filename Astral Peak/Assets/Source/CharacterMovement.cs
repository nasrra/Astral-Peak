using System;
using System.Collections.Generic;
using Entropek;
using UnityEngine;

public class CharacterMovement : Movement{
    public event Action 
        now_grounded, not_grounded, jumped;
    public event Action<GameObject> new_ground;

    [Header("Character Movement")]
    [SerializeField] private bool grounded = false;
    [SerializeField] private bool jumping = false;
    [SerializeField] private bool can_jump = true;
    [SerializeField] private float 
        jump_time, jump_force, jump_force_multiplier, jump_time_counter;
    [SerializeField] private Collider2DFeedback ground_checker;
    List<GameObject> ground = new List<GameObject>();
    public string current_ground_tag {get; private set;}

    void Start() => link();
    void OnDestroy() => unlink();

    public void set_jump_force(float force) => jump_force = force;

    public bool is_grounded() => grounded;

    public override void clear_move_direction(){
        jumping = false;
        end_jump();
        base.clear_move_direction();
    }

    public override void renew(){
        jumping = false;
        jump_time_counter = 0.0f;     
        base.renew();
    }

    // ground check
    private void is_grounded(Collider2D other){
        ground.Add(other.gameObject);
        new_ground?.Invoke(other.gameObject);
        current_ground_tag = other.tag;
        grounded = true;    
        jump_time_counter = 0.0f; 

        // invoke that we are now grounded if we are not toughing other ground objects.
        if(ground.Count == 1){
            check_jump();
            now_grounded?.Invoke();
        }
    }

    public bool check_grounded() => grounded;

    private void is_not_grounded(Collider2D other){
        ground.Remove(other.gameObject);

        // if we are no longer touching any other ground objects.
        if(ground.Count <= 0){
            grounded = false;
            not_grounded?.Invoke();
            reset_decel();
        }
        else
            new_ground?.Invoke(ground[ground.Count-1]);
    }

    // jump command
    public void set_jumping(bool x) => jumping = x;

    public void jump() {
        if(grounded == true && can_jump == true && is_dashing == false){
            mod_decel(1);
            move_direction.y = 1;
            jumped?.Invoke();
        }
    }

    void check_jump(){
        if(jumping == true)
            jump();
    }

    // return the current ground we are standing on.
    public GameObject get_current_ground() => ground[ground.Count - 1];

    public void end_jump(){
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
        if(jumping == true && Mathf.Abs(move_direction.y) > 0.1f && jump_time_counter < jump_time){
        // if(jump_time_counter > 0){
            // if the jump has not exceeded its max height, keeping apply force.
            if(jump_time_counter < jump_time){
                rb.linearVelocity = new Vector2(
                    rb.linearVelocity.x, 
                    move_direction.y * jump_force - (jump_time_counter * jump_force_multiplier) // adding multipler for 'feel'.
                );
                jump_time_counter += Time.deltaTime;
            }
        }
        else if(grounded==false){
                rb.linearVelocity = new Vector2(
                    rb.linearVelocity.x, 
                    rb.linearVelocity.y - 0.66f // adding multipler for 'feel'.
                );
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

    public override void freeform_move_to(Transform target, Action _callback = null){
        jump_time_counter = 0;
        jumping = true;
    }

    public override void freeform_approach_to(Transform target, Action _callback = null){
        base.freeform_approach_to(target);
        jump_time_counter = 0;
        jumping = true;
    }

    protected override void decelerate(){
        // decelerate when grounded and not moving.
        if(grounded == true && Mathf.Abs(move_direction.x) < 0.1f)
            rb.linearVelocity *= data.decel;        
    } 

    public void is_jumpable(int x) => can_jump = x != 0;

    protected override void link(){
        base.link();
        ground_checker.trigger_enter += is_grounded;
        ground_checker.trigger_exit += is_not_grounded;
        dashed += end_jump;
        dash_end += check_jump;
    }

    protected override void unlink(){
        base.unlink();
        ground_checker.trigger_enter -= is_grounded;
        ground_checker.trigger_exit -= is_not_grounded;
        dashed -= end_jump;
        dash_end -= check_jump;
    }
}