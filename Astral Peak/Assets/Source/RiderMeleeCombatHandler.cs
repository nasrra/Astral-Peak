using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiderMeleeCombatHandler : MonoBehaviour{
    [SerializeField] MeleeHolster
        front_strike;
    public void front_strike_hurt_box(int x) => front_strike.enable_hurt_box(x);
}
