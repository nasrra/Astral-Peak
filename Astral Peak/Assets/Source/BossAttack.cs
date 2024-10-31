using UnityEngine;
using System;

[Serializable]
public class BossAttack : MeleeHolster{
    // chance of that attack occuring.
    public float chance;
    // distance from player that the attack would be considered.
    public float distance;
    // the cooldown for a next attack to be thrown after this one.
    public float cooldown;
    public bool enabled = true;
}
