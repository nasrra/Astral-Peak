using System;
using System.Collections.Generic;

public class CavalryCombat : BossCombat{
    BossAttack
        front_strike = new BossAttack(
            "WolfFrontSlash",
            chance:                50,
            player_distance:       6,
            arena_bound_distance:  2,
            attack_cooldown:       4,
            idle_cooldown:         1,
            combat_cooldown:       1),

        bite = new BossAttack(
            "WolfBite",
            chance:                 50,
            player_distance:        6,
            arena_bound_distance:   10,
            attack_cooldown:        1,
            idle_cooldown:          1,
            combat_cooldown:        1),

        back_strike_forward = new BossAttack(
            "WolfBackSlashForward",
            chance:                 50,
            player_distance:        6,
            arena_bound_distance:   20,
            attack_cooldown:        24,
            idle_cooldown:          1,
            combat_cooldown:        1),
        
        back_strike_backward = new BossAttack(
            "WolfBackSlashBackward",
            chance:                 50,
            player_distance:        6,
            arena_bound_distance:   20,
            attack_cooldown:        24,
            idle_cooldown:          1,
            combat_cooldown:        1),
        
        ground_slam = new BossAttack(
            "WolfSig",
            chance:                 50,
            player_distance:        6,
            arena_bound_distance:   2,
            attack_cooldown:        48,
            idle_cooldown:          1,
            combat_cooldown:        1),
        
        howl = new BossAttack(
            "WolfHowl",
            chance:                 50,
            player_distance:        6,
            arena_bound_distance:   0,
            attack_cooldown:        12,
            idle_cooldown:          1,
            combat_cooldown:        1),
        
        jump_away = new BossAttack(
            "WolfJump",
            chance:                 50,
            player_distance:        6,
            arena_bound_distance:   20,
            attack_cooldown:        10,
            idle_cooldown:          0,
            combat_cooldown:        0);
    protected override void create_movesets(){
        movesets = new Dictionary<string, Action>(){
            {"phase_1",
                //()=>{test(jump_away);
                ()=>{
                front_moveset = new List<BossAttack>(){
                    //front_strike,
                    bite,
                };
                back_moveset = new List<BossAttack>(){
                    //back_strike_forward,
                    //back_strike_backward,
                };
                special_moveset = new List<BossAttack>(){
                    howl,
                    ground_slam,
                    //jump_away,
                };
            }},
        };
    }
}