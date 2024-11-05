using UnityEngine;
using System;
using System.Collections;

[Serializable]
public class BossAttack{
    public BossAttack(int animation_id, float chance, float distance, float attack_cooldown, float combat_cooldown, float idle_cooldown = 0, bool enabled = true){
        this.animation_id = animation_id;
        this.chance = chance;
        this.distance = distance;
        this.attack_cooldown = attack_cooldown;
        this.combat_cooldown = combat_cooldown;
        this.idle_cooldown = combat_cooldown + idle_cooldown + 0.05f;
        this.enabled = enabled;
    }

    public int animation_id;
    // chance of that attack occuring.
    public float chance;
    // distance from player that the attack would be considered.
    public float distance;
    // cooldown for this attack to be thrown again.
    public float attack_cooldown;
    // the cooldown for a next attack to be thrown after this one.
    public float combat_cooldown;
    // the cooldown for how long the boss is on idle animation after an attack.
    public float idle_cooldown;
    // whether or not the attack can be chosen.
    public bool enabled = true;

    // called in BossCombat once the attack is concluded.
    public IEnumerator self_cooldown(){
        enabled = false;
        yield return new WaitForSeconds(attack_cooldown);
        enabled = true;
    }
}
