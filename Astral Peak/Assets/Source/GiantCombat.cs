using System;
using System.Collections.Generic;
using UnityEngine;

public class GiantCombat : BossCombat{
    BossAttack down_slam = new(
        "Giant1DownSlam",
        chance:                50,
        max_player_distance:   5,
        min_player_distance:   0,
        arena_bound_distance:  2,
        attack_cooldown:       16,
        idle_cooldown:         0,
        combat_cooldown:       6
    );


    protected override void create_movesets(){
        movesets = new Dictionary<string, Action>(){
            {"phase_1",()=> Debug.Log(1)},
        };
    }

}
