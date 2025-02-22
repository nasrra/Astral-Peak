using System;
using System.Collections.Generic;

public class Giant2Combat : BossCombat{
    BossAttack
    yell_projectile = new BossAttack(
        "Giant2Projectile",
        chance:                 50,
        max_player_distance:    15,
        min_player_distance:    0,
        arena_bound_distance:   0,
        attack_cooldown:        6,
        idle_cooldown:          0,
        combat_cooldown:        6
    ),
    fist_slam = new(
        "Giant2Slam",
        chance:                 50,
        max_player_distance:    30,
        min_player_distance:    0,
        arena_bound_distance:   0,
        attack_cooldown:        4,
        idle_cooldown:          0,
        combat_cooldown:        2
    ),
    finger_gun = new(
        "Giant2Gun",
        chance:                 50,
        max_player_distance:    30,
        min_player_distance:    0,
        arena_bound_distance:   0,
        attack_cooldown:        16,
        idle_cooldown:          0,
        combat_cooldown:        4
    ),
    hand_clap = new(
        "Giant2Clap",
        chance:                 50,
        max_player_distance:    30,
        min_player_distance:    0,
        arena_bound_distance:   0,
        attack_cooldown:        4,
        idle_cooldown:          0,
        combat_cooldown:        4
    ),
    multi_slam = new(
        "Giant2MultiSlam",
        chance:                 50,
        max_player_distance:    30,
        min_player_distance:    0,
        arena_bound_distance:   0,
        attack_cooldown:        8,
        idle_cooldown:          0,
        combat_cooldown:        6
    )
    ;

    protected override void create_movesets(){
        movesets = new Dictionary<string, Action>(){
            {"phase_1",()=>{
                    special_moveset = new List<BossAttack>(){
                        yell_projectile,
                        //fist_slam,
                        //finger_gun,
                        hand_clap,
                        multi_slam,
                    };
                }
            }
        };
    }
}
