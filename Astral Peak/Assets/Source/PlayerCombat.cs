using System;
using UnityEngine;

public class PlayerCombat : MonoBehaviour{
    public Action melee_hit;
    [SerializeField] MeleeHolster melee_attack = new MeleeHolster();
    public void enable_attack_hurt_box(int x){
        if(x == 1)
            melee_attack.hit_creature += invoke_melee_hit;
        else
            melee_attack.hit_creature -= invoke_melee_hit;
        melee_attack.enable_hurt_box(x);
    }
    public void invoke_melee_hit() => melee_hit?.Invoke();
    public MeleeHolster get_attack() => melee_attack;
}
