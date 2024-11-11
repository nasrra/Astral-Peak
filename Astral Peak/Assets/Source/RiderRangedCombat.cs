using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiderRangedCombat : MonoBehaviour{
    [SerializeField] Turret 
        signature_arrow,
        back_shot_arrow;

    public void fire_signature_arrow() => signature_arrow.fire_once();
    public void fire_back_shot_arrow() => back_shot_arrow.fire_once();
}
