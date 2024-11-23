using UnityEngine;
using System;
using System.Collections;

[Serializable]
public class BossAttack{
    public BossAttack(int animation_id, float chance, float player_distance, float arena_bound_distance, float attack_cooldown, float combat_cooldown, float idle_cooldown = 0, bool enabled = true){
        this.animation_id = animation_id;
        this.chance = chance;
        this.player_distance = player_distance;
        this.arena_bound_distance = arena_bound_distance;
        this.attack_cooldown = attack_cooldown;
        this.combat_cooldown = combat_cooldown;
        this.idle_cooldown = combat_cooldown + idle_cooldown + 0.05f;
        this.enabled = enabled;
    }

    public int animation_id;
    public float 
        chance,
        player_distance,
        arena_bound_distance,
        attack_cooldown,
        combat_cooldown,
        idle_cooldown;
    public bool enabled = true;

    // called in BossCombat once the attack is concluded.
    public IEnumerator self_cooldown(){
        enabled = false;
        yield return new WaitForSeconds(attack_cooldown);
        enabled = true;
    }
}
