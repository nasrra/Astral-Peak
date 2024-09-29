using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//NOTE:
// ai path states account for pathing that is at one level or on the ground.
// flying enemies will break. you will need to add functionality for the y-axis.

public abstract class AiPathFollower<T> : MonoBehaviour where T : Movement{
    [SerializeField] protected int path_index;
    [SerializeField] protected float path_timer, dist_from_origin, return_state_threshold;
    [SerializeField] protected AiPathFollowState state;
    [SerializeField] protected List<AiPath> paths = new List<AiPath>();
    [SerializeField] private T movement;
    [SerializeField] protected Transform origin;
    protected Coroutine coroutine;
    
    //void Start() => link_events(); 
    void Start() => set_state(AiPathFollowState.PATHING);
    void OnDisable() => coroutine_clean_up();
    //void OnDestroy() => unlink_events();

    public void set_state(AiPathFollowState s){
        // reset coroutine adjusted values in preperation for the next coroutine.
        coroutine_clean_up();
        // set new state and execute their respective coroutine.
        state = s;
        switch(s){
            case AiPathFollowState.NONE:
                break;
            case AiPathFollowState.PATHING:
                coroutine = StartCoroutine(pathing_loop());
                break;
            case AiPathFollowState.RETREAT:
                coroutine = StartCoroutine(retreat_loop());
                break;
            default:
                throw new System.Exception(s+": has not been implemented!");
        }
    }

    void coroutine_clean_up(){
        if(coroutine != null)
            StopCoroutine(coroutine);
        movement.stop();      
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

    protected IEnumerator retreat_loop(){
        float curr_dist = dist_to_target();
        while(Mathf.Abs(curr_dist) >= 0.25f){
            curr_dist = dist_to_target();
            movement.stop();
            // if we are not moving right, move right.
            if(curr_dist < 0 && movement.get_move_direction() != new Vector2(1,0))
                movement.move_right(true);
            // if we are not moving left, move left.
            if(curr_dist > 0 && movement.get_move_direction() != new Vector2(-1,0))
                movement.move_left(true);
            yield return null;
        }
        set_state(AiPathFollowState.PATHING);
        yield break;
        float dist_to_target() => (transform.position - origin.position).x;
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
    NONE,
    PATHING,
    RETREAT
}
