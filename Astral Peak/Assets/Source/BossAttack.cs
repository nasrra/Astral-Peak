using UnityEngine;
using System;
using System.Collections;

[Serializable]
public class BossAttack : MeleeHolster{
    // chance of that attack occuring.
    public float chance;
    // distance from player that the attack would be considered.
    public float distance;
    // cooldown for this attack to be thrown again.
    public float attack_cooldown;
    // the cooldown for a next attack to be thrown after this one.
    public float combat_cooldown;
    // whether or not the attack can be chosen.
    public bool enabled = true;

    // called in BossCombat once the attack is concluded.
    public IEnumerator self_cooldown(){
        enabled = false;
        yield return new WaitForSeconds(attack_cooldown);
        enabled = true;
    }
}
