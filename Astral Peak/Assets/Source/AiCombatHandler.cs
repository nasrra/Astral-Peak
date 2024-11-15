using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AiCombatHandler<T> : MonoBehaviour where T : Movement{
    public event Action<string>
        perform_action;
    public event Action 
        target_in_range, target_left_range;
    [SerializeField] protected int moveset_index;
    [SerializeField] protected float cooldown, target_dist;
    [SerializeField] protected AiCombatState state;
    [SerializeField] protected Movement movement;
    [SerializeField] protected Collider2DFeedback combat_range, agro_area;
    [SerializeField] protected Transform target;
    [SerializeField] protected AiCombatAttack chosen_attack;
    [SerializeField] protected List<AiCombatMoveset> movesets = new List<AiCombatMoveset>();
    Coroutine coroutine;

    void Start(){
        link_external();
        link_internal(); 
    } 
    void OnDestroy(){
        unlink_external();
        unlink_internal(); 
    }

    public AiCombatState get_state() => state;

#region States
    private void coroutine_clean_up(){ 
        if(coroutine != null)
            StopCoroutine(coroutine); 
        movement.stop();    
    }

    public void none_state(){
        coroutine_clean_up();
        state = AiCombatState.NONE;
    }

    public void chase_state(){
        coroutine_clean_up();
        state = AiCombatState.CHASE;
        coroutine = StartCoroutine(chase_loop());
    }

    protected IEnumerator chase_loop(){
        while(true){
            target_dist = dist_to_target();

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

            // attempt an attack.
            // keep this at the end of the co routine so the ai can stop moving.
            attack();
            yield return null;
        }
        float dist_to_target() => (transform.position - target.position).x;
    }

    // Note: need to make the chance of the attack applicable 
    // add to the algorithm so that some attacks are more frequently picked
    // depending upon their chance percentage.
    void attack(){

        // lower the attack cooldown.
        cooldown -= Time.deltaTime;

        // regulate, in the case that there is no available attacks on the previous frame.
        if(cooldown <= 0.0f)
            cooldown = 0.0f;

        // if we have not yet recovered from a previous attack, do not attack again.
        if(cooldown > 0.1f)
            return;
        
        List<AiCombatAttack> available_attacks = new List<AiCombatAttack>();
        // loop through the current move set.
        foreach(AiCombatAttack attack in movesets[moveset_index].attacks)
            if(Mathf.Abs(target_dist) <= attack.distance)
                available_attacks.Add(attack);

        // NOTE: if only one attack is available: choose it and skip the bottom code.
        if(available_attacks.Count <= 0)
            return;

        // choose and execute attack.
        coroutine_clean_up();
        state = AiCombatState.ATTACK;
        int index = UnityEngine.Random.Range(0, available_attacks.Count);
        chosen_attack = available_attacks[index]; 
        perform_action?.Invoke(chosen_attack.name);
        cooldown = chosen_attack.cooldown;
    }

    void defend(){
        // if we are attacking, do not parry.
        if(state == AiCombatState.ATTACK || state == AiCombatState.NONE || state == AiCombatState.DEFEND)
            return;
        coroutine_clean_up();
        state = AiCombatState.DEFEND;
        perform_action?.Invoke("guard");
    }

    // used for when a state has finished
    // and the next state is uncertain.
    public AiCombatState recovery_state(){
        if(target!= null) 
            return AiCombatState.CHASE; 
        else
            return AiCombatState.NONE;
    }

    public AiCombatAttack get_chosen_attack() => chosen_attack;
#endregion
#region Linkage
    void on_target_enter(Collider2D c){
        target = c.transform;
        target_in_range?.Invoke();    
    }
    void on_target_exit(Collider2D c){
        target = null;
        target_left_range?.Invoke();
    } 

    public void link_external(){
        agro_area.trigger_enter += on_target_enter;
        agro_area.trigger_exit += on_target_exit; 
        //link_input();
    }

    public void unlink_external(){
        agro_area.trigger_enter -= on_target_enter;
        agro_area.trigger_exit -= on_target_exit;
        //unlink_input();
    }

    public void link_internal(){
        target_in_range += chase_state;
        target_left_range += none_state;
    }

    public void unlink_internal(){
        target_in_range -= chase_state;
        target_left_range -= none_state;
    }

    private void link_input() => InputManager.attack_performed += defend;
    private void unlink_input() => InputManager.attack_performed -= defend;
#endregion
}

[System.Serializable]
public enum AiCombatState{
    NONE,
    CHASE,
    ATTACK,
    DEFEND
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