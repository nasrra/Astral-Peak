using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Deluz;

public abstract class BossCombat : MonoBehaviour{
    public event Action<float> attack_ended;
    public event Action<BossAttack> attack_chosen;
    [SerializeField] public bool on_cooldown = false, is_attacking = false;
    [SerializeField] BossAttack chosen_attack;
    [SerializeField] protected List<BossAttack> front_moveset   = new List<BossAttack>();
    [SerializeField] protected List<BossAttack> back_moveset    = new List<BossAttack>(); 
    [SerializeField] protected List<BossAttack> special_moveset = new List<BossAttack>();
    protected Dictionary<string, Action> movesets;
    [SerializeField] public Transform  
        left_arena_bound,
        right_arena_bound;
    Coroutine state;

    void Awake() => create_movesets();
    private void state_switch(IEnumerator _state){
        if(state != null)
            StopCoroutine(state);
        state = _state!=null? StartCoroutine(_state) : null;
    }
    public void set_moveset(string _moveset) => movesets[_moveset]();
    public void halt() => state_switch(null);
    public void renew(){
        on_cooldown = false;
        is_attacking = false;
    }
    protected abstract void create_movesets();
    // Note: need to make the chance of the attack applicable 
    // add to the algorithm so that some attacks are more frequently picked
    // depending upon their chance percentage.
    public void chose_attack_state(Transform target) => state_switch(chose_attack(target));
    protected IEnumerator chose_attack(Transform target){
        while(target != null){
            // return if there are no attacks moves available.
            if(on_cooldown == true || (front_moveset.Count == 0 && back_moveset.Count == 0 && special_moveset.Count == 0)){
                yield return new WaitForFixedUpdate();
                continue;
            }

            float target_distance = transform.position.x - target.position.x;
            float abs_distance = Mathf.Abs(target_distance);
            float left_bound_distance = dist_to_left_bound();
            float right_bound_distance = dist_to_left_bound(); // used to be left btw.

            // Determine the appropriate moveset based on the boss's orientation and player's position.
            List<BossAttack> available_attacks = 
                (transform.rotation.y == 0 && target_distance <= 0) || (transform.rotation.y != 0 && target_distance >= 0) 
                ? get_available_attacks(front_moveset , abs_distance, left_bound_distance, right_bound_distance)
                : get_available_attacks(back_moveset, abs_distance, left_bound_distance, right_bound_distance);

            // Get the list of available attacks from the primary moveset and add special attacks.
            available_attacks.AddRange(get_available_attacks(special_moveset, abs_distance, left_bound_distance, right_bound_distance));

            // choose and execute attack.
            if(available_attacks.Count <= 0){
                yield return new WaitForFixedUpdate();
                continue;
            }
            int index = UnityEngine.Random.Range(0, available_attacks.Count);
            chosen_attack = available_attacks[index]; 
            attack_chosen?.Invoke(chosen_attack);
            is_attacking = true;
            state_switch(null);
            yield break;
        }
        yield break;
    }

    public BossAttack get_chosen_attack() => chosen_attack;

    public void attack_end(){
        is_attacking = false;
        BossAttack attack = chosen_attack; // here to remove referencing bug with enabled timer.
        StartCoroutine(Util.timer(attack.attack_cooldown, start_action:()=>attack.enabled = false, time_out:()=>attack.enabled = true));
        StartCoroutine(Util.timer(attack.combat_cooldown, start_action:()=>on_cooldown=true, time_out:()=>on_cooldown=false));
        attack_ended?.Invoke(chosen_attack.idle_cooldown);
    }

    public List<BossAttack> get_available_attacks(List<BossAttack> attacks, float td, float lbd, float rbd){
        List<BossAttack> available_attacks = new List<BossAttack>();

        foreach(BossAttack attack in attacks)
            if(attack.enabled == true 
                && td <= attack.player_distance
                && (transform.rotation.y == 0 && lbd >= attack.arena_bound_distance || 
                    transform.rotation.y != 0 && rbd >= attack.arena_bound_distance))
                available_attacks.Add(attack);
        return available_attacks;
    }

    float dist_to_left_bound() => Mathf.Abs(left_arena_bound.position.x - transform.position.x);
    float dist_to_right_bound() => Mathf.Abs(right_arena_bound.position.x - transform.position.x);

    protected void test(BossAttack attack){
        attack.player_distance = 10;
        attack.arena_bound_distance = 0;
        attack.combat_cooldown = 1;
        attack.attack_cooldown = 1;
        special_moveset = new List<BossAttack>(){attack};
    }
}