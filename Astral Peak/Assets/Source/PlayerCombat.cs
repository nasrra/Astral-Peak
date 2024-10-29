using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour{
    [SerializeField] MeleeHolster melee_attack = new MeleeHolster(PlayerAnimator.ATTACK);
    public int attack() => melee_attack.animation_id;
    public void emit_attack_particles() => melee_attack.emit_particles();
    public void enable_attack_hurt_box(int x) => melee_attack.enable_hurt_box(x);
    public MeleeHolster get_attack() => melee_attack;
}
