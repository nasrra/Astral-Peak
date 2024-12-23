using UnityEngine;
using System.Collections.Generic;

public class MageCombat : BossCombat{
    BossAttack
        projectile_summon = new BossAttack(
            "projectile_summon",
            chance:                50,
            player_distance:       6,
            arena_bound_distance:  2,
            attack_cooldown:       4,
            idle_cooldown:         0,
            combat_cooldown:       2),
        teleport = new BossAttack(
            "enter_teleport",
            chance:                50,
            player_distance:       6,
            arena_bound_distance:  0,
            attack_cooldown:       4,
            idle_cooldown:         1,
            combat_cooldown:       2),
        hollow_summon = new BossAttack(
            "hollow_summon",
            chance:                50,
            player_distance:       6,
            arena_bound_distance:  6,
            attack_cooldown:       12,
            idle_cooldown:         3,
            combat_cooldown:       3)
        ;

    void Start(){
        test(hollow_summon);
        //set_movesets(); 
    }

    void set_movesets(){
        set_special_moveset(new List<BossAttack>(){
            projectile_summon,
            hollow_summon,
            teleport,
        });
    }
}
