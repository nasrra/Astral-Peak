using System.Collections.Generic;

public class CavalryCombat : BossCombat{
    BossAttack
        front_strike = new BossAttack(
            "front_strike",
            chance:                50,
            player_distance:       6,
            arena_bound_distance:  2,
            attack_cooldown:       4,
            idle_cooldown:         1,
            combat_cooldown:       1),

        bite = new BossAttack(
            "bite",
            chance:                 50,
            player_distance:        6,
            arena_bound_distance:   10,
            attack_cooldown:        1,
            idle_cooldown:          1,
            combat_cooldown:        1),

        back_strike_forward = new BossAttack(
            "back_strike_forward",
            chance:                 50,
            player_distance:        6,
            arena_bound_distance:   20,
            attack_cooldown:        24,
            idle_cooldown:          1,
            combat_cooldown:        1),
        
        back_strike_backward = new BossAttack(
            "back_strike_backward",
            chance:                 50,
            player_distance:        6,
            arena_bound_distance:   20,
            attack_cooldown:        24,
            idle_cooldown:          1,
            combat_cooldown:        1),
        
        ground_slam = new BossAttack(
            "ground_slam",
            chance:                 50,
            player_distance:        6,
            arena_bound_distance:   2,
            attack_cooldown:        48,
            idle_cooldown:          1,
            combat_cooldown:        1),
        
        howl = new BossAttack(
            "howl",
            chance:                 50,
            player_distance:        6,
            arena_bound_distance:   0,
            attack_cooldown:        12,
            idle_cooldown:          1,
            combat_cooldown:        1),
        
        jump_away = new BossAttack(
            "jump_away",
            chance:                 50,
            player_distance:        6,
            arena_bound_distance:   20,
            attack_cooldown:        10,
            idle_cooldown:          0,
            combat_cooldown:        0);

    void Start(){
        //test(back_strike_backward);
        set_movesets(); 
    }

    void set_movesets(){
        set_front_moveset(new List<BossAttack>(){
            front_strike,
            bite,
            //fetch,
        });

        set_back_moveset(new List<BossAttack>(){
            back_strike_forward,
            back_strike_backward,
        });
    
        set_special_moveset(new List<BossAttack>(){
            howl,
            ground_slam,
            jump_away,
        });
    }
}