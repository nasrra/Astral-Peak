using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCombat : MonoBehaviour{
    public event Action<float> attack_ended;
    [SerializeField] public float cooldown;
    [SerializeField] BossAttack chosen_attack;
    [SerializeField] protected List<BossAttack> front_moveset   = new List<BossAttack>();
    [SerializeField] protected List<BossAttack> back_moveset    = new List<BossAttack>(); 
    [SerializeField] protected List<BossAttack> special_moveset = new List<BossAttack>();
    Coroutine timer;

    // used as an animator key event.
    public void set_front_moveset   (List<BossAttack> moveset) => front_moveset = moveset;
    public void set_back_moveset    (List<BossAttack> moveset) => back_moveset = moveset;
    public void set_special_moveset (List<BossAttack> moveset) => special_moveset = moveset;

    // Note: need to make the chance of the attack applicable 
    // add to the algorithm so that some attacks are more frequently picked
    // depending upon their chance percentage.
    public BossAttack chose_attack(float dist_to_target){
        // get available attacks depending on where the player is located.
        List<BossAttack> available_attacks; 
        
        // get a list of the attacks depeding upon the bosses orientation and whether or not the player is behind or in front of the boss.
        if(transform.rotation.y == 0)
            available_attacks = dist_to_target >= 0? available_behind_attacks(dist_to_target) : available_infront_attacks(dist_to_target);        
        else
            available_attacks = dist_to_target <= 0? available_behind_attacks(dist_to_target) : available_infront_attacks(dist_to_target); 

        // add special attacks to the available attacks.
        available_attacks.AddRange(available_special_attacks(dist_to_target));

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

    public List<BossAttack> available_infront_attacks(float dist_to_target){
        List<BossAttack> available_attacks = new List<BossAttack>();
        foreach(BossAttack attack in front_moveset)
            if(attack.enabled == true && Mathf.Abs(dist_to_target) <= attack.distance)
                available_attacks.Add(attack);
        return available_attacks;
    }

    public List<BossAttack> available_behind_attacks(float dist_to_target){
        List<BossAttack> available_attacks = new List<BossAttack>();
        foreach(BossAttack attack in back_moveset)
            if(attack.enabled == true && Mathf.Abs(dist_to_target) <= attack.distance)
                available_attacks.Add(attack);
        return available_attacks;
    }

    public List<BossAttack> available_special_attacks(float dist_to_target){
        List<BossAttack> available_attacks = new List<BossAttack>();
        foreach(BossAttack attack in special_moveset)
            if(attack.enabled == true && Mathf.Abs(dist_to_target) <= attack.distance)
                available_attacks.Add(attack);
        return available_attacks;        
    }

    protected void test(BossAttack attack){
        attack.distance = 10;
        attack.combat_cooldown = 1;
        attack.attack_cooldown = 1;
        set_special_moveset(new List<BossAttack>(){
            attack,
        });
    }
}