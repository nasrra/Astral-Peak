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
            combat_cooldown:       1)
    ;
    void Start(){
        test(projectile_summon);
        //set_movesets(); 
    }

    void set_movesets(){
        set_front_moveset(new List<BossAttack>(){
            //front_disengage,
        });
    }
}
