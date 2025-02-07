using System;
using System.Collections.Generic;

public class Giant2Combat : BossCombat{
    BossAttack
    yell_projectile = new BossAttack(
        "Giant2HeadYellProjectile",
        chance:                 50,
        max_player_distance:    15,
        min_player_distance:    0,
        arena_bound_distance:   0,
        attack_cooldown:        4,
        idle_cooldown:          0,
        combat_cooldown:        2
    ),
    fist_slam = new(
        "Giant2FistSlam",
        chance:                 50,
        max_player_distance:    30,
        min_player_distance:    0,
        arena_bound_distance:   0,
        attack_cooldown:        4,
        idle_cooldown:          0,
        combat_cooldown:        2
    ),
    finger_gun = new(
        "Giant2FingerGun",
        chance:                 50,
        max_player_distance:    30,
        min_player_distance:    0,
        arena_bound_distance:   0,
        attack_cooldown:        16,
        idle_cooldown:          0,
        combat_cooldown:        4
    )
    ;

    protected override void create_movesets(){
        movesets = new Dictionary<string, Action>(){
            {"phase_1",()=>{
                    special_moveset = new List<BossAttack>(){
                        yell_projectile,
                        fist_slam,
                        finger_gun,
                    };
                }
            }
        };
    }
}
