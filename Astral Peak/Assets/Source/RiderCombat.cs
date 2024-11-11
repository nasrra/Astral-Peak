using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RiderCombat : BossCombat{
    BossAttack
        front_strike = new BossAttack(
            RiderAnimator.FRONT_STRIKE,
            chance: 50,
            distance: 12,
            attack_cooldown: 4,
            idle_cooldown: 0,
            combat_cooldown: 1
        ),
        signature = new BossAttack(
            RiderAnimator.SIGNATURE,
            chance: 50,
            distance: 12,
            attack_cooldown: 4,
            idle_cooldown: 0,
            combat_cooldown: 1
        ),
        run_n_gun = new BossAttack(
            RiderAnimator.RUN_N_GUN,
            chance: 50,
            distance: 12,
            attack_cooldown: 4,
            idle_cooldown: 0,
            combat_cooldown: 1
        ),
        jump_n_dash = new BossAttack(
            RiderAnimator.JUMP_N_DASH,
            chance: 50,
            distance: 12,
            attack_cooldown: 4,
            idle_cooldown: 0,
            combat_cooldown: 1
        ),
        back_shot = new BossAttack(
            RiderAnimator.BACK_SHOT,
            chance: 50,
            distance: 12,
            attack_cooldown: 4,
            idle_cooldown: 0,
            combat_cooldown: 1            
        );

    void Start(){
        //test(front_strike);
        set_movesets();
    }

    void set_movesets(){
        set_front_moveset(new List<BossAttack>(){
            signature,
        });

        set_back_moveset(new List<BossAttack>(){
            back_shot,
        });
    
        set_special_moveset(new List<BossAttack>(){
            jump_n_dash,
        });
    }
}
