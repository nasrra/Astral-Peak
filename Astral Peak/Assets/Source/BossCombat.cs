using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCombat : MonoBehaviour{
    [SerializeField] public float cooldown;
    [SerializeField] BossAttack chosen_attack;
    [SerializeField] protected Dictionary<int, BossAttack> front_moveset = new Dictionary<int, BossAttack>();
    [SerializeField] protected Dictionary<int, BossAttack> back_moveset = new Dictionary<int, BossAttack>(); 
    [SerializeField] protected Dictionary<int, BossAttack> special_moveset = new Dictionary<int, BossAttack>();
    Coroutine timer;

    // used as an animator key event.
    protected void set_front_moveset(Dictionary<int, BossAttack> moveset) => front_moveset = moveset;
    protected void set_back_moveset(Dictionary<int, BossAttack> moveset) => back_moveset = moveset;
    protected void set_special_moveset(Dictionary<int, BossAttack> moveset) => special_moveset = moveset;

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

    // animator key event functions.
    public void enable_attack_hurt_box(int x) => chosen_attack.enable_hurt_box(x);
    public void emit_attack_particles() => chosen_attack.emit_particles();

    IEnumerator cooldown_timer(float time){
        cooldown = time;
        yield return new WaitForSeconds(cooldown);
        cooldown = 0;
        yield break;
    }

    public List<BossAttack> available_infront_attacks(float dist_to_target){
        List<BossAttack> available_attacks = new List<BossAttack>();
        foreach(KeyValuePair<int, BossAttack> attack in front_moveset)
            if(Mathf.Abs(dist_to_target) <= attack.Value.distance)
                available_attacks.Add(attack.Value);
        return available_attacks;
    }

    public List<BossAttack> available_behind_attacks(float dist_to_target){
        List<BossAttack> available_attacks = new List<BossAttack>();
        foreach(KeyValuePair<int, BossAttack> attack in back_moveset)
            if(Mathf.Abs(dist_to_target) <= attack.Value.distance)
                available_attacks.Add(attack.Value);
        return available_attacks;
    }

    public void flip_particles_right(){
        foreach(KeyValuePair<int, BossAttack> a in front_moveset)
            a.Value.flip_particle_emitter_right();
        foreach(KeyValuePair<int, BossAttack> a in back_moveset)
            a.Value.flip_particle_emitter_right();
        foreach(KeyValuePair<int, BossAttack> a in special_moveset)
            a.Value.flip_particle_emitter_right();
    }

    public void flip_particles_left(){
        foreach(KeyValuePair<int, BossAttack> a in front_moveset)
            a.Value.flip_particle_emitter_left();
        foreach(KeyValuePair<int, BossAttack> a in back_moveset)
            a.Value.flip_particle_emitter_left();
        foreach(KeyValuePair<int, BossAttack> a in special_moveset)
            a.Value.flip_particle_emitter_left();
    }
}