using System.Collections.Generic;

public class RiderCombat : BossCombat{
    BossAttack
        front_strike = new BossAttack(
            "front_strike",
            chance:                 50,
            player_distance:        12,
            arena_bound_distance:   7,
            attack_cooldown:        4,
            idle_cooldown:          2,
            combat_cooldown:        1
        ),
        signature = new BossAttack(
            "signature",
            chance:                 50,
            player_distance:        5,
            arena_bound_distance:   7,
            attack_cooldown:        4,
            idle_cooldown:          3,
            combat_cooldown:        1
        ),
        jump_n_dash = new BossAttack(
            "jump_n_dash",
            chance:                 50,
            player_distance:        12,
            arena_bound_distance:   10,
            attack_cooldown:        4,
            idle_cooldown:          2,
            combat_cooldown:        1
        ),
        back_shot = new BossAttack(
            "back_shot",
            chance:                 50,
            player_distance:        12,
            arena_bound_distance:   0,
            attack_cooldown:        4,
            idle_cooldown:          0,
            combat_cooldown:        1            
        ),
        round_shot = new BossAttack(
            "round_shot",
            chance:                 50,
            player_distance:        6,
            arena_bound_distance:   0,
            attack_cooldown:        8,
            idle_cooldown:          2,
            combat_cooldown:        1
        );

    void Start(){
        //test(round_shot);
        set_movesets();
    }

    void set_movesets(){
        set_front_moveset(new List<BossAttack>(){
            signature,
            jump_n_dash, 
        });

        set_back_moveset(new List<BossAttack>(){
            round_shot,
        });
    
        set_special_moveset(new List<BossAttack>(){       
        });
    }
}
