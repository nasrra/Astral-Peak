using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiderRangedCombat : MonoBehaviour{
    [SerializeField] Turret 
        signature_arrow,
        back_shot_arrow;
    [SerializeField]
        List<Turret> round_shot_arrows;

    public void fire_signature_arrow()          => signature_arrow.fire_once();
    public void fire_back_shot_arrow()          => back_shot_arrow.fire_once();
    public void fire_round_shot_arrow(int x)    => round_shot_arrows[x].fire_once();
}
