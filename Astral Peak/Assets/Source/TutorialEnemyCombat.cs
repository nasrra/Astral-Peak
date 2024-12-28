using System.Collections.Generic;
using UnityEngine;
using System;

public class TutorialEnemyCombat : BossCombat{
    public event Action<Collider2D> player_in_range, player_left_range;
    [SerializeField] Collider2DFeedback agro_area;

    void Start(){
        agro_area.trigger_enter += in_range;
        agro_area.trigger_exit += left_range;
    }

    void OnDestroy(){
        agro_area.trigger_enter -= in_range;
        agro_area.trigger_exit  -= left_range;       
    }

    void in_range(Collider2D col) => player_in_range?.Invoke(col);
    void left_range(Collider2D col) => player_left_range?.Invoke(col);

    protected override void create_movesets(){}
}
