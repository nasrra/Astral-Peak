using UnityEngine;
using System.Collections.Generic;

public class MageCombat : BossCombat{
    BossAttack
        front_disengage = new BossAttack(
            "front_disengage",
            chance:                50,
            player_distance:       6,
            arena_bound_distance:  2,
            attack_cooldown:       4,
            idle_cooldown:         0,
            combat_cooldown:       1)
    ;
    void Start(){
        //test(jump_away);
        //set_movesets(); 
    }

    void set_movesets(){
        set_front_moveset(new List<BossAttack>(){
            front_disengage,
        });
    }
}
