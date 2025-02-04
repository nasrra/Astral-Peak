using System;
using System.Collections.Generic;

public class GiantCombat : BossCombat{
    BossAttack down_slam = new(
        "GiantDownSlam",
        chance:                50,
        max_player_distance:   5,
        min_player_distance:   0,
        arena_bound_distance:  2,
        attack_cooldown:       6,
        idle_cooldown:         2,
        combat_cooldown:       6
    );


    protected override void create_movesets(){
        movesets = new Dictionary<string, Action>(){
            {"phase_1",()=> {
                    front_moveset = new List<BossAttack>(){
                        down_slam
                    };
                }
            },
        };
    }

}
