using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiderMeleeCombatHandler : MonoBehaviour{
    [SerializeField] MeleeHolster
        front_strike,
        signature_strike,
        jump_n_dash_strike;

    public void front_strike_hurt_box(int x) => front_strike.enable_hurt_box(x);
    public void signature_strike_hurt_box(int x) => signature_strike.enable_hurt_box(x);
    public void jump_n_dash_strike_hurt_box(int x) => jump_n_dash_strike.enable_hurt_box(x);
}
