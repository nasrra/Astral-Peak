using System.Collections.Generic;
using System.Diagnostics.Tracing;

public class CavalryCombat : BossCombat{
    BossAttack
        none = new BossAttack(
            1,
            chance:             50,
            distance:           1000,
            attack_cooldown:    1000,
            idle_cooldown:      1000,
            combat_cooldown:    1000),

        front_strike = new BossAttack(
            CavalryAnimator.FRONT_STRIKE,
            chance:             50,
            distance:           6,
            attack_cooldown:    4,
            idle_cooldown:      0,
            combat_cooldown:    1),

        bite = new BossAttack(
            CavalryAnimator.BITE_1,
            chance:             50,
            distance:           6,
            attack_cooldown:    1,
            idle_cooldown:      0,
            combat_cooldown:    1),
        
        fetch = new BossAttack(
            CavalryAnimator.FETCH_1,
            chance:             50,
            distance:           6,
            attack_cooldown:    24,
            idle_cooldown:      0,
            combat_cooldown:    1),
        
        back_strike_forward = new BossAttack(
            CavalryAnimator.BACK_STRIKE_FORWARD,
            chance:             50,
            distance:           6,
            attack_cooldown:    24,
            idle_cooldown:      0,
            combat_cooldown:    1),
        
        back_strike_backward = new BossAttack(
            CavalryAnimator.BACK_STRIKE_BACKWARD,
            chance:             50,
            distance:           6,
            attack_cooldown:    24,
            idle_cooldown:      0,
            combat_cooldown:    1),
        
        ground_slam = new BossAttack(
            CavalryAnimator.GROUND_SLAM,
            chance:             50,
            distance:           6,
            attack_cooldown:    48,
            idle_cooldown:      0,
            combat_cooldown:    1),
        
        sword_summon = new BossAttack(
            CavalryAnimator.HOWL,
            chance:             50,
            distance:           6,
            attack_cooldown:    12,
            idle_cooldown:      0,
            combat_cooldown:    1),
        
        jump_away = new BossAttack(
            CavalryAnimator.JUMP_AWAY,
            chance:             50,
            distance:           6,
            attack_cooldown:    10,
            idle_cooldown:      0,
            combat_cooldown:    0);

    void Start(){
        //test(sword_summon);
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
            sword_summon,
            ground_slam,
            jump_away,
        });
    }
}