using System;
using System.Collections.Generic;

public class MageCombat : BossCombat{
    BossAttack
        projectile_summon_phase_1 = new BossAttack(
            "Mage1ProjSum",
            chance:                50,
            player_distance:       6,
            arena_bound_distance:  2,
            attack_cooldown:       4,
            idle_cooldown:         0,
            combat_cooldown:       2),
        teleport_phase_1 = new BossAttack(
            "Mage1EnterTel",
            chance:                50,
            player_distance:       6,
            arena_bound_distance:  0,
            attack_cooldown:       4,
            idle_cooldown:         1,
            combat_cooldown:       2),
        hollow_summon_phase_1 = new BossAttack(
            "Mage1HolSum",
            chance:                50,
            player_distance:       6,
            arena_bound_distance:  6,
            attack_cooldown:       12,
            idle_cooldown:         3,
            combat_cooldown:       3),
        three_slash_phase_1 = new BossAttack(
            "Mage1ThreeSlash",
            chance:                50,
            player_distance:       6,
            arena_bound_distance:  2,
            attack_cooldown:       4,
            idle_cooldown:         0,
            combat_cooldown:       2),
        hollow_summon_phase_2 = new BossAttack(
            "Mage2HolSum",
            chance:                50,
            player_distance:       6,
            arena_bound_distance:  6,
            attack_cooldown:       9, //24
            idle_cooldown:         3,
            combat_cooldown:       8),
        projectile_summon_phase_2 = new BossAttack(
            "Mage2ProjSum",
            chance:                50,
            player_distance:       20,
            arena_bound_distance:  2,
            attack_cooldown:       8,
            idle_cooldown:         0,
            combat_cooldown:       6),
        signature_phase_2 = new BossAttack(
            "Mage2Sig",
            chance:                50,
            player_distance:       20,
            arena_bound_distance:  2,
            attack_cooldown:       9,
            idle_cooldown:         0,
            combat_cooldown:       8),
        left_right_phase_2 = new BossAttack(
            "Mage2LeftRight",
            chance:                50,
            player_distance:       20,
            arena_bound_distance:  2,
            attack_cooldown:       8,
            idle_cooldown:         0,
            combat_cooldown:       6),
        right_left_phase_2 = new BossAttack(
            "Mage2RightLeft",
            chance:                50,
            player_distance:       20,
            arena_bound_distance:  2,
            attack_cooldown:       8,
            idle_cooldown:         0,
            combat_cooldown:       6)
        ;
    protected override void create_movesets(){
        movesets = new Dictionary<int, Action>(){
            {1,()=>{
                front_moveset = new List<BossAttack>();
                back_moveset = new List<BossAttack>();
                special_moveset = new List<BossAttack>(){
                    //projectile_summon_phase_1,
                    teleport_phase_1,
                    //hollow_summon_phase_1,
                    three_slash_phase_1,
                };
            }},
            {2,()=>{
                front_moveset = new List<BossAttack>();
                back_moveset = new List<BossAttack>();
                special_moveset = new List<BossAttack>(){
                    //projectile_summon_phase_2,
                    hollow_summon_phase_2,
                    signature_phase_2,
                    //left_right_phase_2,
                    //right_left_phase_2,
                };
            }}
        };
    }
}
