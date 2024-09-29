using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public abstract class AiPathFollower<T> : MonoBehaviour where T : Movement{
    [SerializeField] protected bool right_from_origin;
    [SerializeField] protected int path_index;
    [SerializeField] protected float path_timer, dist_from_origin, return_state_threshold;
    [SerializeField] protected AiPathFollowState state;
    [SerializeField] protected AiPath current_path;
    [SerializeField] protected List<AiPath> paths = new List<AiPath>();
    [SerializeField] protected Collider2DFeedback agro_area;
    [SerializeField] protected Transform origin; // where the enemy is placed.
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
        agro_area.trigger_enter += player_in_range;
        agro_area.trigger_exit += player_left_range;
    }

    void unlink_events(){
        agro_area.trigger_enter -= player_in_range;
        agro_area.trigger_exit -= player_left_range;
    }

    void player_in_range(Collider2D c = null) => set_state(AiPathFollowState.CHASE);
    void player_left_range(Collider2D c = null) => set_state(AiPathFollowState.PATHING); 

    void set_state(AiPathFollowState s){
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

    void pathing(){
        follow_path(true);
    }

    void chase(){
        follow_path(false);
    }

    void retreat(){
        follow_path(false);
    }

    public void follow_path(bool x){
        if(paths.Count <= 0)
            return;
        if(x == true)
            coroutine = StartCoroutine(loop());
        else if(coroutine != null)
            StopCoroutine(coroutine);
    }

    protected IEnumerator loop(){
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
