using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiderRangedCombat : MonoBehaviour{
    [SerializeField] Turret 
        front_strike_arrow;

    public void fire_front_strike_arrow() => front_strike_arrow.fire_once();
}
