using System;
using System.Collections.Generic;
using UnityEngine;

public class BossCombat : MonoBehaviour{
    [SerializeField] public float cooldown;
    [SerializeField] BossAttack chosen_attack;
    [SerializeField] public List<BossAttack> moveset = new List<BossAttack>();
    
    // used as an animator key event.
    public void set_moveset(List<BossAttack> moveset) => this.moveset = moveset;

    void FixedUpdate(){
        // lower the attack cooldown.
        cooldown -= Time.deltaTime;

        // regulate, in the case that there is no available attacks on the previous frame.
        if(cooldown <= 0.0f)
            cooldown = 0.0f;        
    }

    // Note: need to make the chance of the attack applicable 
    // add to the algorithm so that some attacks are more frequently picked
    // depending upon their chance percentage.
    public BossAttack chose_attack(float dist_to_target){
        List<BossAttack> available_attacks = new List<BossAttack>();
        // loop through the current move set.
        foreach(BossAttack attack in moveset)
            if(Mathf.Abs(dist_to_target) <= attack.distance)
                available_attacks.Add(attack);

        // NOTE: if only one attack is available: choose it and skip the bottom code.
        //if(available_attacks.Count <= 0)
        //    return null;

        // choose and execute attack.
        int index = UnityEngine.Random.Range(0, available_attacks.Count);
        chosen_attack = available_attacks[index]; 
        cooldown = chosen_attack.cooldown;
        return chosen_attack;
    }
}

[Serializable]
public struct BossAttack{
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
