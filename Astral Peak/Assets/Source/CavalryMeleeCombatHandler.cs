using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class CavalryMeleeCombatHandler : MonoBehaviour{
    [SerializeField] MeleeHolster
        front_strike,
        back_strike_backward,
        back_strike_forward,
        bite;

    // animator events;
    public void front_strike_hurt_box(bool x) => front_strike.enable_hurt_box(x);
    public void back_strike_backward_hurt_box(bool x) => back_strike_backward.enable_hurt_box(x);
    public void back_strike_forward_hurt_box(bool x) => back_strike_forward.enable_hurt_box(x);
    public void bite_hurt_box(bool x) => bite.enable_hurt_box(x);
}
