using System;
using System.Collections.Generic;
using Entropek;

public class RiderCombat : BossCombat{
    BossAttack
        signature = new BossAttack(
            "RiderSignature",
            chance:                 50,
            max_player_distance:    4,
            min_player_distance:    0,
            arena_bound_distance:   7,
            attack_cooldown:        4,
            idle_cooldown:          2,
            combat_cooldown:        2
        ),
        jump_n_dash = new BossAttack(
            "RiderJumpNDash",
            chance:                 50,
            max_player_distance:    4,
            min_player_distance:    0,
            arena_bound_distance:   10,
            attack_cooldown:        4,
            idle_cooldown:          2,
            combat_cooldown:        1
        ),
        round_shot = new BossAttack(
            "RiderRoundShot",
            chance:                 50,
            max_player_distance:    4,
            min_player_distance:    0,
            arena_bound_distance:   0,
            attack_cooldown:        8,
            idle_cooldown:          2,
            combat_cooldown:        1
        ),
        front_jump_forward = new BossAttack(
            "RiderJumpForward",
            chance:                 50,
            max_player_distance:    12,
            min_player_distance:    9,
            arena_bound_distance:   0,
            attack_cooldown:        16,
            idle_cooldown:          0,
            combat_cooldown:        1
        ),
        back_jump_forward = new BossAttack(
            "RiderJumpForward",
            chance:                 50,
            max_player_distance:    4,
            min_player_distance:    0,
            arena_bound_distance:   0,
            attack_cooldown:        8,
            idle_cooldown:          0,
            combat_cooldown:        1
        ),
        jump_backward = new BossAttack(
            "RiderJumpBackward",
            chance:                 50,
            max_player_distance:    5,
            min_player_distance:    0,
            arena_bound_distance:   0,
            attack_cooldown:        16,
            idle_cooldown:          0,
            combat_cooldown:        1
        ),  
        walk_n_fire = new BossAttack(
            "RiderWalkNFire",
            chance:                 50,
            max_player_distance:    6,
            min_player_distance:    0,
            arena_bound_distance:   0,
            attack_cooldown:        8,
            idle_cooldown:          0,
            combat_cooldown:        2
        )
        ;
        protected override void create_movesets(){
            movesets = new Dictionary<string, Action>(){
                {"phase_1",()=>{
                    front_moveset = new List<BossAttack>(){
                        jump_backward,
                        signature,
                        front_jump_forward,
                        walk_n_fire,
                    };
                    back_moveset = new List<BossAttack>(){
                        back_jump_forward,     
                    };
                    special_moveset = new List<BossAttack>(){
                        round_shot,      
                    };
                }},
            };
        }

}
