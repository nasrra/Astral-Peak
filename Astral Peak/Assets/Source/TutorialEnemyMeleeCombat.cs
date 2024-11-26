using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialEnemyMeleeCombat : MonoBehaviour{
    [SerializeField] MeleeHolster front_strike;
    public void enable_front_strike_hurt_box(int x) => front_strike.enable_hurt_box(x); 
}
