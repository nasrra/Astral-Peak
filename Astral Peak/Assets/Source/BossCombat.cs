using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCombat : MonoBehaviour{
    [SerializeField] public float cooldown;
    [SerializeField] BossAttack chosen_attack;
    [SerializeField] public List<BossAttack> front_moveset = new List<BossAttack>();
    [SerializeField] public List<BossAttack> back_moveset = new List<BossAttack>();
    [SerializeField] public List<BossAttack> special_moveset = new List<BossAttack>();
    Coroutine timer;

    // used as an animator key event.
    public void set_front_moveset(List<BossAttack> moveset) => front_moveset = moveset;
    public void set_back_moveset(List<BossAttack> moveset) => back_moveset = moveset;
    public void set_special_moveset(List<BossAttack> moveset) => special_moveset = moveset;

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

        // choose and execute attack.
        if(available_attacks.Count <= 0)
            return null;
        int index = UnityEngine.Random.Range(0, available_attacks.Count);
        chosen_attack = available_attacks[index]; 
        StartCoroutine(cooldown_timer(chosen_attack.cooldown));
        return chosen_attack;
    }

    IEnumerator cooldown_timer(float time){
        cooldown = time;
        yield return new WaitForSeconds(cooldown);
        cooldown = 0;
        yield break;
    }

    public List<BossAttack> available_infront_attacks(float dist_to_target){
        //Debug.Log("front");
        List<BossAttack> available_attacks = new List<BossAttack>();
        foreach(BossAttack attack in front_moveset)
            if(Mathf.Abs(dist_to_target) <= attack.distance)
                available_attacks.Add(attack);
        return available_attacks;
    }

    public List<BossAttack> available_behind_attacks(float dist_to_target){
        //Debug.Log("back");
        List<BossAttack> available_attacks = new List<BossAttack>();
        foreach(BossAttack attack in back_moveset)
            if(Mathf.Abs(dist_to_target) <= attack.distance)
                available_attacks.Add(attack);
        return available_attacks;
    }
}

[Serializable]
public class BossAttack{
    public BossAttack(int animation_id, float distance, float cooldown){
        this.animation_id = animation_id;
        this.distance = distance;
        this.cooldown = cooldown;
    }
    // the name of the trigger in the animation tree to play.
    public int animation_id;
    // chance of that attack occuring.
    // public float chance;
    // distance from player that the attack would be considered.
    public float distance;
    // the cooldown for a next attack to be thrown after this one.
    public float cooldown;
}
