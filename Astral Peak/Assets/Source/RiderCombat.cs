using System;
using System.Collections.Generic;
using Entropek;

public class RiderCombat : BossCombat{
    BossAttack
        front_strike = new BossAttack(
            "front_strike",
            chance:                 50,
            player_distance:        4,
            arena_bound_distance:   7,
            attack_cooldown:        4,
            idle_cooldown:          2,
            combat_cooldown:        1
        ),
        signature = new BossAttack(
            "signature",
            chance:                 50,
            player_distance:        4,
            arena_bound_distance:   7,
            attack_cooldown:        4,
            idle_cooldown:          3,
            combat_cooldown:        1
        ),
        jump_n_dash = new BossAttack(
            "jump_n_dash",
            chance:                 50,
            player_distance:        4,
            arena_bound_distance:   10,
            attack_cooldown:        4,
            idle_cooldown:          2,
            combat_cooldown:        1
        ),
        back_shot = new BossAttack(
            "back_shot",
            chance:                 50,
            player_distance:        4,
            arena_bound_distance:   0,
            attack_cooldown:        4,
            idle_cooldown:          0,
            combat_cooldown:        1            
        ),
        round_shot = new BossAttack(
            "round_shot",
            chance:                 50,
            player_distance:        4,
            arena_bound_distance:   0,
            attack_cooldown:        8,
            idle_cooldown:          2,
            combat_cooldown:        1
        );
        protected override void create_movesets(){
            movesets = new Dictionary<string, Action>(){
                {"phase_1",()=>{
                    test(signature);
                    //front_moveset = new List<BossAttack>(){
                    //    signature,
                    //    jump_n_dash, 
                    //};
                    //back_moveset = new List<BossAttack>(){
                    //    round_shot,             
                    //};
                    //special_moveset = new List<BossAttack>();
                }},
            };
        }

}
