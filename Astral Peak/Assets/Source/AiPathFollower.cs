using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AiPathFollower<T> : MonoBehaviour where T : Movement{
    [SerializeField] protected bool right_from_origin;
    [SerializeField] protected int path_index;
    [SerializeField] protected float path_timer, dist_from_origin, return_state_threshold;
    [SerializeField] protected AiPathFollowState state;
    [SerializeField] protected List<AiPath> paths = new List<AiPath>();
    [SerializeField] protected Collider2DFeedback agro_area;
    [SerializeField] protected Transform origin, target;
    [SerializeField] private T movement;
    protected Coroutine coroutine;
    
    void Start(){
        link_events();
        set_state(AiPathFollowState.PATHING);
    } 

    void OnDestroy(){
        unlink_events();
    }

    void link_events(){
        agro_area.trigger_enter += target_in_range;
        agro_area.trigger_exit += target_left_range;
    }

    void unlink_events(){
        agro_area.trigger_enter -= target_in_range;
        agro_area.trigger_exit -= target_left_range;
    }

    void target_in_range(Collider2D c){
        target = c.transform;
        set_state(AiPathFollowState.CHASE);
    } 
    void target_left_range(Collider2D c){
        target = null;
        set_state(AiPathFollowState.PATHING);
    } 

    void set_state(AiPathFollowState s){
        // reset coroutine adjusted values in preperation for the next coroutine.
        coroutine_clean_up();
        // set new state and execute their respective coroutine.
        state = s;
        switch(s){
            case AiPathFollowState.PATHING:
                pathing();
                break;
            case AiPathFollowState.CHASE:
                chase();
                break;
            case AiPathFollowState.RETREAT:
                retreat();
                break;
            default:
                throw new System.Exception(s+": has not been implemented!");
        }
    }

    void coroutine_clean_up(){
        switch(state){
            case AiPathFollowState.PATHING:
                break;
            case AiPathFollowState.CHASE:
                movement.stop();
                break;
            case AiPathFollowState.RETREAT:
                break;
            default:
                throw new System.Exception(state+": has not been implemented!");
        }        
    }

    void pathing(){
        if(coroutine != null)
            StopCoroutine(coroutine);
        coroutine = StartCoroutine(pathing_loop());
    }

    protected IEnumerator pathing_loop(){
        // initialize
        AiPath current_path = paths[path_index];
        path_index = 0;
        path_timer = 0;

        while(true){
            // start movement.
            if(path_timer == 0.0f){
                current_path = paths[path_index];
                movement.movement(current_path.get_movement(), true);
                path_timer = current_path.get_duration();
            }
            // count down duration timer.
            else if(path_timer > 0.0f)
                path_timer -= Time.deltaTime;
            // stop movement.
            else{
                path_timer = 0.0f;
                movement.movement(current_path.get_movement(), false);
                path_index = ((path_index + 1) >= paths.Count)? 0 : path_index + 1;
            }
            yield return null;
        }
    }

    void chase(){
        if(coroutine != null)
            StopCoroutine(coroutine);
        coroutine = StartCoroutine(chase_loop());
    }

    protected IEnumerator chase_loop(){
        while(true){
            float curr_dist = dist_to_target();
            Debug.Log(curr_dist);
            // if we are not moving right, move right.
            if(curr_dist < 0 && movement.get_move_direction() != new Vector2(1,0)){
                movement.stop();
                movement.move_right(true);
            }
            // if we are not moving left, move left.
            if(curr_dist > 0 && movement.get_move_direction() != new Vector2(-1,0)){
                movement.stop();
                movement.move_left(true);
            }
            yield return null;
        }
        float dist_to_target() => (transform.position - target.position).x;
    }

    void retreat(){
        if(coroutine != null)
            StopCoroutine(coroutine);
    }

    void FixedUpdate(){
        //relation_to_origin();
        //if(dist_from_origin > return_state_threshold)
        //    return_to_origin();
    }

    void relation_to_origin(){
        // calc direction.
        float direction = (transform.position - origin.position).x;
        right_from_origin = direction > 0.0f;
        // calc distance.
        dist_from_origin = Mathf.Abs(direction);
    } 

    void return_to_origin(){
        if(dist_from_origin > return_state_threshold)
            if(right_from_origin == true)
                movement.move_left(true);
    }
}

// Path that the Ai will follow.
[System.Serializable]
public struct AiPath{
    // direction to move in.
    [SerializeField] private MovementOption movement;
    // duration of movement.
    [SerializeField] private float duration;
    public MovementOption get_movement() => movement;
    public float get_duration() => duration; 
}

public enum AiPathFollowState{
    PATHING,
    CHASE,
    RETREAT
}
