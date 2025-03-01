using System;
using System.Collections.Generic;

public class Giant1Combat : BossCombat{
    BossAttack down_slam = new(
        "Giant1DownSlam",
        max_player_distance:   10,
        min_player_distance:   0,
        arena_bound_distance:  2,
        attack_cooldown:       6,
        idle_cooldown:         0,
        combat_cooldown:       2
    ),
    jump_backward = new BossAttack(
        "Giant1JumpBackward",
        max_player_distance:    10,
        min_player_distance:    0,
        arena_bound_distance:   0,
        attack_cooldown:        16,
        idle_cooldown:          0,
        combat_cooldown:        1
    ),
    front_jump_forward = new BossAttack(
        "Giant1JumpForward",
        max_player_distance:    16,
        min_player_distance:    8,
        arena_bound_distance:   0,
        attack_cooldown:        16,
        idle_cooldown:          0,
        combat_cooldown:        1
    ),
    back_jump_forward = new BossAttack(
        "Giant1JumpForward",
        max_player_distance:    8,
        min_player_distance:    0,
        arena_bound_distance:   0,
        attack_cooldown:        2,
        idle_cooldown:          0,  
        combat_cooldown:        1
    ),
    round_slam = new BossAttack(
        "Giant1RoundSlam",
        max_player_distance:    10,
        min_player_distance:    0,
        arena_bound_distance:   0,
        attack_cooldown:        24,
        idle_cooldown:          1,
        combat_cooldown:        2
    ),
    walk_projectile = new BossAttack(
        "Giant1WalkingProjectile",
        max_player_distance:    15,
        min_player_distance:    0,
        arena_bound_distance:   0,
        attack_cooldown:        8,
        idle_cooldown:          0,
        combat_cooldown:        2
    ),
    geyser = new BossAttack(
        "Giant1Geyser",
        max_player_distance:    15,
        min_player_distance:    0,
        arena_bound_distance:   0,
        attack_cooldown:        8,
        idle_cooldown:          0,
        combat_cooldown:        2
    ),
    three_piece = new BossAttack(
        "Giant1ThreePiece",
        max_player_distance:    15,
        min_player_distance:    0, 
        arena_bound_distance:   0,
        attack_cooldown:        10,
        idle_cooldown:          0,
        combat_cooldown:        3
    )
    ;
    protected override void create_movesets(){
        movesets = new Dictionary<string, Action>(){
            {"phase_1",()=> {
                    // test(walk_projectile);
                    front_moveset = new List<BossAttack>(){
                       down_slam,
                       jump_backward,
                       front_jump_forward,
                       three_piece,
                    };
                    back_moveset = new List<BossAttack>(){
                       back_jump_forward,
                    };
                    special_moveset = new List<BossAttack>(){
                       walk_projectile,
                       round_slam,
                       geyser,
                    };
                }
            },
        };
    }

}
