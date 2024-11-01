using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CavalryCombat : BossCombat{
    [SerializeField] CavalryAnimator animator;
    [SerializeField] BossAttack
        front_strike            = new BossAttack(),
        bite                    = new BossAttack(),
        fetch                   = new BossAttack(),
        back_strike_forward     = new BossAttack(),
        back_strike_backward    = new BossAttack(),
        ground_slam             = new BossAttack(),
        sword_summon            = new BossAttack();

    void Start(){
        front_strike.set_animation(()=>animator.front_strike());
        back_strike_forward.set_animation(()=>animator.back_strike_forward());
        back_strike_backward.set_animation(()=>animator.back_strike_backward());
        bite.set_animation(()=>animator.bite_1());
        fetch.set_animation(()=>animator.fetch_1());
        sword_summon.set_animation(()=>animator.howl());
        ground_slam.set_animation(()=>animator.ground_slam());

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
}