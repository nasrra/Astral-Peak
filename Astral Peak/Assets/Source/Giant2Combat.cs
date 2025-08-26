using System;
using System.Collections.Generic;

public class Giant2Combat : BossCombat{
    BossAttack
    yell_projectile = new BossAttack(
        "Giant2Projectile",
        max_player_distance:    15,
        min_player_distance:    0,
        arena_bound_distance:   0,
        attack_cooldown:        6,
        idle_cooldown:          0,
        combat_cooldown:        0.5f
    ),
    fist_slam = new(
        "Giant2Slam",
        max_player_distance:    30,
        min_player_distance:    0,
        arena_bound_distance:   0,
        attack_cooldown:        4,
        idle_cooldown:          0,
        combat_cooldown:        1f
    ),
    finger_gun = new(
        "Giant2Gun",
        max_player_distance:    30,
        min_player_distance:    0,
        arena_bound_distance:   0,
        attack_cooldown:        16,
        idle_cooldown:          0,
        combat_cooldown:        1f
    ),
    hand_clap = new(
        "Giant2Clap",
        max_player_distance:    30,
        min_player_distance:    0,
        arena_bound_distance:   0,
        attack_cooldown:        4,
        idle_cooldown:          0,
        combat_cooldown:        1f
    ),
    multi_slam = new(
        "Giant2MultiSlam",
        max_player_distance:    30,
        min_player_distance:    0,
        arena_bound_distance:   0,
        attack_cooldown:        8,
        idle_cooldown:          0,
        combat_cooldown:        1f
    ),
    geyser = new(
        "Giant2Geyser",
        max_player_distance: 35,
        min_player_distance: 0,
        arena_bound_distance: 0,
        attack_cooldown:      8,
        combat_cooldown:      0.5f
    )
    ;

    protected override void create_movesets(){
        movesets = new Dictionary<string, Action>(){
            {"phase_1",()=>{
                    // test(geyser);
                    special_moveset = new List<BossAttack>(){
                        yell_projectile,
                        fist_slam,
                        finger_gun,
                        hand_clap,
                        multi_slam,
                        geyser,
                    };
                }
            }
        };
    }
}
