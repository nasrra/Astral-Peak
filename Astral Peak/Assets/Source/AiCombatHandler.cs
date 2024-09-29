using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public abstract class AiCombatHandler<T> : MonoBehaviour where T : Movement{
    public event Action target_in_range, target_left_range;
    [SerializeField] protected AiCombatState state;
    [SerializeField] protected Movement movement;
    [SerializeField] protected Collider2DFeedback combat_range, agro_area;
    [SerializeField] protected Transform target;
    Coroutine coroutine;

    void Start() => link_events(); 
    void OnEnable() => set_state(AiCombatState.NONE);
    void OnDisable() => coroutine_clean_up();
    void OnDestroy() => unlink_events();

    void link_events(){
        agro_area.trigger_enter += on_target_enter;
        agro_area.trigger_exit += on_target_exit;        
    }

    void unlink_events(){
        agro_area.trigger_enter -= on_target_enter;
        agro_area.trigger_exit -= on_target_exit;
    }

    public void set_state(AiCombatState s){
        // reset coroutine adjusted values in preperation for the next coroutine.
        coroutine_clean_up();
        // set new state and execute their respective coroutine.
        state = s;
        switch(s){
            case AiCombatState.NONE:
                break;
            case AiCombatState.CHASE:
                coroutine = StartCoroutine(chase_loop());
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

    protected IEnumerator chase_loop(){
        while(true){
            float curr_dist = dist_to_target();
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

    void on_target_enter(Collider2D c){
        target = c.transform;
        target_in_range?.Invoke();
        set_state(AiCombatState.CHASE);        
    }
    void on_target_exit(Collider2D c){
        target = null;
        target_left_range?.Invoke();
        set_state(AiCombatState.NONE);
    } 
}


public enum AiCombatState{
    NONE,
    CHASE,
    ATTACK,
    DEFEND
}