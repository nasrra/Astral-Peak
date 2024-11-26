using System.Collections.Generic;
using UnityEngine;
using System;

public class TutorialEnemyCombat : BossCombat{
    public event Action player_in_range, player_left_range;
    [SerializeField] Collider2DFeedback agro_area;

    void Start(){
        set_special_moveset(new List<BossAttack>{
            new BossAttack(
                HollowAnimator.ATTACK,
                chance: 100, 
                player_distance: 2.5f, 
                arena_bound_distance: 0,
                attack_cooldown: 0, 
                combat_cooldown: 1,
                idle_cooldown: 0
            ),
        });
        agro_area.trigger_enter += in_range;
        agro_area.trigger_exit += left_range;
    }

    void OnDestroy(){
        agro_area.trigger_enter -= in_range;
        agro_area.trigger_exit  -= left_range;       
    }

    void in_range(Collider2D col) => player_in_range?.Invoke();
    void left_range(Collider2D col) => player_left_range?.Invoke();
}
