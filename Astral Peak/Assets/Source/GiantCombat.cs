using System;
using System.Collections.Generic;

public class GiantCombat : BossCombat{
    BossAttack down_slam = new(
        "GiantDownSlam",
        chance:                50,
        max_player_distance:   10,
        min_player_distance:   0,
        arena_bound_distance:  2,
        attack_cooldown:       6,
        idle_cooldown:         2,
        combat_cooldown:       6
    ),
    jump_backward = new BossAttack(
        "GiantJumpBackward",
        chance:                 50,
        max_player_distance:    10,
        min_player_distance:    0,
        arena_bound_distance:   0,
        attack_cooldown:        16,
        idle_cooldown:          2,
        combat_cooldown:        4
    ),
    front_jump_forward = new BossAttack(
        "GiantJumpForward",
        chance:                 50,
        max_player_distance:    16,
        min_player_distance:    9,
        arena_bound_distance:   0,
        attack_cooldown:        16,
        idle_cooldown:          2,
        combat_cooldown:        4
    )
    ;
    protected override void create_movesets(){
        movesets = new Dictionary<string, Action>(){
            {"phase_1",()=> {
                    front_moveset = new List<BossAttack>(){
                        down_slam,
                        jump_backward,
                        front_jump_forward,
                    };
                }
            },
        };
    }

}
