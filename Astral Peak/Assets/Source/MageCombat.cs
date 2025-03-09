using System;
using System.Collections.Generic;

public class MageCombat : BossCombat{
    BossAttack
        projectile_summon_phase_1 = new(
            "Mage1ProjSum",
            max_player_distance:   6,
            min_player_distance:   0,
            arena_bound_distance:  2,
            attack_cooldown:       4,
            idle_cooldown:         0,
            combat_cooldown:       2),
        teleport_phase_1 = new(
            "Mage1EnterTel",
            max_player_distance:   6,
            min_player_distance:   0,
            arena_bound_distance:  8,
            attack_cooldown:       4,
            idle_cooldown:         1,
            combat_cooldown:       2),
        hollow_summon_phase_1 = new(
            "Mage1HolSum",
            max_player_distance:   6,
            min_player_distance:   0,
            arena_bound_distance:  6,
            attack_cooldown:       24,
            idle_cooldown:         0,
            combat_cooldown:       2),
        three_slash_phase_1 = new(
            "Mage1ThreeSlash",
            max_player_distance:   6,
            min_player_distance:   0,
            arena_bound_distance:  2,
            attack_cooldown:       4,
            idle_cooldown:         0,
            combat_cooldown:       2),
        hollow_summon_phase_2 = new(
            "Mage2HolSum",
            max_player_distance:   6,
            min_player_distance:   0,
            arena_bound_distance:  6,
            attack_cooldown:       40, //24
            idle_cooldown:         3,
            combat_cooldown:       3.5f),
        projectile_summon_phase_2 = new(
            "Mage2ProjSum",
            max_player_distance:   20,
            min_player_distance:   0,
            arena_bound_distance:  2,
            attack_cooldown:       8,
            idle_cooldown:         0,
            combat_cooldown:       3.2f),
        signature_phase_2 = new(
            "Mage2Sig",
            max_player_distance:   20,
            min_player_distance:   0,
            arena_bound_distance:  2,
            attack_cooldown:       9,
            idle_cooldown:         0,
            combat_cooldown:       3.2f),
        left_right_phase_2 = new(
            "Mage2LeftRight",
            max_player_distance:   20,
            min_player_distance:   0,
            arena_bound_distance:  2,
            attack_cooldown:       16,
            idle_cooldown:         0,
            combat_cooldown:       2.5f),
        right_left_phase_2 = new(
            "Mage2RightLeft",
            max_player_distance:   20,
            min_player_distance:   0,
            arena_bound_distance:  2,
            attack_cooldown:       16,
            idle_cooldown:         0,
            combat_cooldown:       2.5f)
        ;
    protected override void create_movesets(){
        movesets = new Dictionary<string, Action>(){
            {"phase_1",()=>{
                front_moveset = new List<BossAttack>();
                back_moveset = new List<BossAttack>();
                special_moveset = new List<BossAttack>(){
                    projectile_summon_phase_1,
                    teleport_phase_1,
                    hollow_summon_phase_1,
                    three_slash_phase_1,
                };
            }},
            {"phase_2",()=>{
                front_moveset = new List<BossAttack>();
                back_moveset = new List<BossAttack>();
                special_moveset = new List<BossAttack>(){
                    hollow_summon_phase_2,
                    projectile_summon_phase_2,
                    signature_phase_2,
                    left_right_phase_2,     
                    right_left_phase_2,
                };
            }}
        };
    }
}
