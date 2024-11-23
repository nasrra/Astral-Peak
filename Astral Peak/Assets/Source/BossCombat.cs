using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BossCombat : MonoBehaviour{
    public event Action<float> attack_ended;
    [SerializeField] public float cooldown;
    [SerializeField] BossAttack chosen_attack;
    [SerializeField] protected List<BossAttack> front_moveset   = new List<BossAttack>();
    [SerializeField] protected List<BossAttack> back_moveset    = new List<BossAttack>(); 
    [SerializeField] protected List<BossAttack> special_moveset = new List<BossAttack>();
    [SerializeField] Transform  
        left_arena_bound,
        right_arena_bound;

    // used as an animator key event.
    public void set_front_moveset   (List<BossAttack> moveset) => front_moveset = moveset;
    public void set_back_moveset    (List<BossAttack> moveset) => back_moveset = moveset;
    public void set_special_moveset (List<BossAttack> moveset) => special_moveset = moveset;

    // Note: need to make the chance of the attack applicable 
    // add to the algorithm so that some attacks are more frequently picked
    // depending upon their chance percentage.
    public BossAttack chose_attack(float dist_to_target){
        float target_distance = Mathf.Abs(dist_to_target);
        float left_bound_distance = dist_to_left_bound();
        float right_bound_distance = dist_to_left_bound();

        // Determine the appropriate moveset based on the boss's orientation and player's position.
        List<BossAttack> available_attacks = 
            (transform.rotation.y == 0 && dist_to_target >= 0) || (transform.rotation.y != 0 && dist_to_target <= 0) 
            ? get_available_attacks(back_moveset , target_distance, left_bound_distance, right_bound_distance)
            : get_available_attacks(front_moveset, target_distance, left_bound_distance, right_bound_distance);

        // Get the list of available attacks from the primary moveset and add special attacks.
        available_attacks.AddRange(get_available_attacks(special_moveset, target_distance, left_bound_distance, right_bound_distance));


        // choose and execute attack.
        if(available_attacks.Count <= 0)
            return null;
        int index = UnityEngine.Random.Range(0, available_attacks.Count);
        chosen_attack = available_attacks[index]; 
        return chosen_attack;
    }

    public void attack_end(){
        attack_ended?.Invoke(chosen_attack.idle_cooldown);
        StartCoroutine(chosen_attack.self_cooldown());
        StartCoroutine(cooldown_timer(chosen_attack.combat_cooldown));
    }

    IEnumerator cooldown_timer(float time){
        cooldown = time;
        yield return new WaitForSeconds(cooldown);
        cooldown = 0;
        yield break;
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
        set_special_moveset(new List<BossAttack>(){
            attack,
        });
    }
}