using System.Collections.Generic;

public class CavalryCombat : BossCombat{
    BossAttack
        front_strike = new BossAttack(
            CavalryAnimator.FRONT_STRIKE,
            chance:             50,
            distance:           6,
            attack_cooldown:    4,
            combat_cooldown:    1),

        bite = new BossAttack(
            CavalryAnimator.BITE_1,
            chance:             50,
            distance:           6,
            attack_cooldown:    1,
            combat_cooldown:    1),
        
        fetch = new BossAttack(
            CavalryAnimator.FETCH_1,
            chance:             50,
            distance:           6,
            attack_cooldown:    24,
            combat_cooldown:    1),
        
        back_strike_forward = new BossAttack(
            CavalryAnimator.BACK_STRIKE_FORWARD,
            chance:             50,
            distance:           6,
            attack_cooldown:    24,
            combat_cooldown:    1),
        
        back_strike_backward = new BossAttack(
            CavalryAnimator.BACK_STRIKE_BACKWARD,
            chance:             50,
            distance:           6,
            attack_cooldown:    24,
            combat_cooldown:    1),
        
        ground_slam = new BossAttack(
            CavalryAnimator.GROUND_SLAM,
            chance:             50,
            distance:           6,
            attack_cooldown:    48,
            combat_cooldown:    1),
        
        sword_summon = new BossAttack(
            CavalryAnimator.HOWL,
            chance:             50,
            distance:           6,
            attack_cooldown:    12,
            combat_cooldown:    1);

    void Start(){
        test(bite);
        //set_movesets(); 
    }

    void set_movesets(){
        set_front_moveset(new List<BossAttack>(){
            front_strike,
            bite,
            fetch,
        });

        set_back_moveset(new List<BossAttack>(){
            back_strike_forward,
            back_strike_backward,
        });
    
        set_special_moveset(new List<BossAttack>(){
            sword_summon,
            ground_slam,
        });
    }

    void test(BossAttack attack){
        attack.distance = 10;
        attack.combat_cooldown = 1;
        attack.attack_cooldown = 1;
        set_special_moveset(new List<BossAttack>(){
            attack,
        });
    }
}