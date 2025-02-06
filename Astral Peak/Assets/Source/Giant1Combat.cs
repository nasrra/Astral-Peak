using System;
using System.Collections.Generic;

public class Giant1Combat : BossCombat{
    BossAttack down_slam = new(
        "Giant1DownSlam",
        chance:                50,
        max_player_distance:   10,
        min_player_distance:   0,
        arena_bound_distance:  2,
        attack_cooldown:       6,
        idle_cooldown:         3,
        combat_cooldown:       3
    ),
    jump_backward = new BossAttack(
        "Giant1JumpBackward",
        chance:                 50,
        max_player_distance:    10,
        min_player_distance:    0,
        arena_bound_distance:   0,
        attack_cooldown:        16,
        idle_cooldown:          2,
        combat_cooldown:        4
    ),
    front_jump_forward = new BossAttack(
        "Giant1JumpForward",
        chance:                 50,
        max_player_distance:    16,
        min_player_distance:    8,
        arena_bound_distance:   0,
        attack_cooldown:        16,
        idle_cooldown:          2,
        combat_cooldown:        4
    ),
    back_jump_forward = new BossAttack(
        "Giant1JumpForward",
        chance:                 50,
        max_player_distance:    8,
        min_player_distance:    0,
        arena_bound_distance:   0,
        attack_cooldown:        2,
        idle_cooldown:          0,
        combat_cooldown:        1
    ),
    round_slam = new BossAttack(
        "Giant1RoundSlam",
        chance:                 50,
        max_player_distance:    10,
        min_player_distance:    0,
        arena_bound_distance:   0,
        attack_cooldown:        24,
        idle_cooldown:          3,
        combat_cooldown:        6
    ),
    walk_projectile = new BossAttack(
        "Giant1WalkingProjectile",
        chance:                 50,
        max_player_distance:    15,
        min_player_distance:    0,
        arena_bound_distance:   0,
        attack_cooldown:        8,
        idle_cooldown:          0,
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
                    back_moveset = new List<BossAttack>(){
                        back_jump_forward,
                    };
                    special_moveset = new List<BossAttack>(){
                        walk_projectile,
                        round_slam,
                    };
                }
            },
        };
    }

}
