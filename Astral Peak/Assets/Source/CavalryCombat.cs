using System;
using System.Collections.Generic;

public class CavalryCombat : BossCombat{
    BossAttack
        front_strike = new BossAttack(
            "WolfFrontSlash",
            max_player_distance:    6,
            min_player_distance:    0,
            arena_bound_distance:   2,
            attack_cooldown:        4,
            idle_cooldown:          1,
            combat_cooldown:        1),

        bite = new BossAttack(
            "WolfBite",
            max_player_distance:    6,
            min_player_distance:    0,
            arena_bound_distance:   10,
            attack_cooldown:        1,
            idle_cooldown:          1,
            combat_cooldown:        1),

        back_strike_forward = new BossAttack(
            "WolfBackSlashForward",
            max_player_distance:    6,
            min_player_distance:    0,
            arena_bound_distance:   20,
            attack_cooldown:        24,
            idle_cooldown:          1,
            combat_cooldown:        1),
        
        back_strike_backward = new BossAttack(
            "WolfBackSlashBackward",
            max_player_distance:    6,
            min_player_distance:    0,
            arena_bound_distance:   20,
            attack_cooldown:        24,
            idle_cooldown:          1,
            combat_cooldown:        1),
        
        ground_slam = new BossAttack(
            "WolfSig",
            max_player_distance:    6,
            min_player_distance:    0,
            arena_bound_distance:   2,
            attack_cooldown:        48,
            idle_cooldown:          1,
            combat_cooldown:        1),
        
        howl = new BossAttack(
            "WolfHowl",
            max_player_distance:    6,
            min_player_distance:    0,
            arena_bound_distance:   0,
            attack_cooldown:        12,
            idle_cooldown:          1,
            combat_cooldown:        1),
        
        jump_away = new BossAttack(
            "WolfJump",
            max_player_distance:    6,
            min_player_distance:    0,
            arena_bound_distance:   20,
            attack_cooldown:        10,
            idle_cooldown:          0,
            combat_cooldown:        0);
    protected override void create_movesets(){
        movesets = new Dictionary<string, Action>(){
            {"phase_1",
                ()=>{
                    test(ground_slam);
                    //front_moveset = new List<BossAttack>(){
                    //    front_strike,
                    //    bite,
                    //};
                    //special_moveset = new List<BossAttack>(){
                    //    howl,
                    //    ground_slam,
                    //    jump_away,
                    //};
                }
            },
        };
    }
}