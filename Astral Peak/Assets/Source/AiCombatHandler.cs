using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AiCombatHandler<T> : MonoBehaviour where T : Movement{
    public event Action target_in_range, target_left_range;
    [SerializeField] protected bool attacking = false;
    [SerializeField] protected int moveset_index;
    [SerializeField] protected float cooldown, target_dist;
    [SerializeField] protected AiCombatState state;
    [SerializeField] protected Movement movement;
    [SerializeField] protected Collider2DFeedback combat_range, agro_area;
    [SerializeField] protected Transform target;
    [SerializeField] protected List<AiCombatMoveset> movesets = new List<AiCombatMoveset>();
    [SerializeField] protected Animator animator;
    Coroutine coroutine;

    void Start() => link_events(); 
    void OnEnable() => set_state(AiCombatState.NONE);
    void OnDisable() => coroutine_clean_up();
    void OnDestroy() => unlink_events();

    private void link_events(){
        agro_area.trigger_enter += on_target_enter;
        agro_area.trigger_exit += on_target_exit;        
    }

    private void unlink_events(){
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

    private void coroutine_clean_up(){
        if(coroutine != null)
            StopCoroutine(coroutine);
        movement.stop();      
    }

    protected IEnumerator chase_loop(){
        while(true){
            target_dist = dist_to_target();
            
            // lower the attack cooldown.
            cooldown -= Time.deltaTime;
            // attempt an attack.
            attack();
            
            // dont chase if we are attacking.
            if(attacking == true)
                yield return null; 

            // if we are not moving right, move right.
            if(target_dist < 0 && movement.get_move_direction() != new Vector2(1,0)){
                movement.stop();
                movement.move_right(true);
            }
            // if we are not moving left, move left.
            if(target_dist > 0 && movement.get_move_direction() != new Vector2(-1,0)){
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

    // Note: need to make the chance of the attack applicable 
    // add to the algorithm so that some attacks are more frequently picked
    // depending upon their chance percentage.
    void attack(){

        // regulate, in the case that there is no available attacks on the previous frame.
        if(cooldown <= 0.0f)
            cooldown = 0.0f;

        // if we have not yet recovered from a previous attack, do not attack again.
        if(cooldown > 0.1f)
            return;
        
        // list of available attacks to choose from.
        List<AiCombatAttack> available_attacks = new List<AiCombatAttack>();
        
        // loop through the current move set.
        foreach(AiCombatAttack attack in movesets[moveset_index].attacks)
            if(Mathf.Abs(target_dist) <= attack.distance)
                available_attacks.Add(attack);

        // if only one attack is available: choose it and skip the bottom code.

        if(available_attacks.Count <= 0)
            return;

        // choose an attack.
        int index = UnityEngine.Random.Range(0, available_attacks.Count);
    
        // execute that attack.
        AiCombatAttack chosen = available_attacks[index]; 
        movement.stop();
        animator.SetTrigger(chosen.name);
        cooldown = chosen.cooldown;
    }

    // this function is used for animation key events.
    public void is_attacking(int x) => attacking = x != 0;
}

[System.Serializable]
public struct AiCombatMoveset{
    public List<AiCombatAttack> attacks;
}

[System.Serializable]
public struct AiCombatAttack{
    // the name of the trigger in the animation tree to play.
    public string name;
    // chance of that attack occuring.
    // public float chance;
    // distance from player that the attack would be considered.
    public float distance;
    // the cooldown for a next attack to be thrown after this one.
    public float cooldown;
}

[System.Serializable]
public enum AiCombatState{
    NONE,
    CHASE,
    ATTACK,
    DEFEND
}