using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiderCombat : BossCombat{
    BossAttack
        front_strike = new BossAttack(
            RiderAnimator.FRONT_STRIKE_1,
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
        );

    void Start(){
        test(front_strike);
        //set_movesets();
    }

    void set_movesets(){
        set_front_moveset(new List<BossAttack>(){
            front_strike,
            run_n_gun,
        });

        set_back_moveset(new List<BossAttack>(){
        });
    
        set_special_moveset(new List<BossAttack>(){
        });
    }
}
