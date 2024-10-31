using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCombat : MonoBehaviour{
    public event Action attack_ended;
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

        // choose and execute attack.
        if(available_attacks.Count <= 0)
            return null;
        int index = UnityEngine.Random.Range(0, available_attacks.Count);
        chosen_attack = available_attacks[index]; 
        return chosen_attack;
    }

    // animator key event functions.
    public void enable_attack_hurt_box(int x) => chosen_attack.enable_hurt_box(x);
    public void emit_attack_particles() => chosen_attack.emit_particles();
    public void attack_end(){
        attack_ended?.Invoke();
        StartCoroutine(cooldown_timer(chosen_attack.cooldown));
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

    public void flip_particles_right(){
        foreach(BossAttack a in front_moveset)
            a.flip_particle_emitter_right();
        foreach(BossAttack a in back_moveset)
            a.flip_particle_emitter_right();
        foreach(BossAttack a in special_moveset)
            a.flip_particle_emitter_right();
    }

    public void flip_particles_left(){
        foreach(BossAttack a in front_moveset)
            a.flip_particle_emitter_left();
        foreach(BossAttack a in back_moveset)
            a.flip_particle_emitter_left();
        foreach(BossAttack a in special_moveset)
            a.flip_particle_emitter_left();
    }
}