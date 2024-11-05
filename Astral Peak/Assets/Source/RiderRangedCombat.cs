using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiderRangedCombat : MonoBehaviour{
    [SerializeField] Turret 
        sword_summon_1,
        sword_summon_2,
        sword_summon_3,
        sword_summon_4,
        sword_summon_5;

    public void summon_swords(){
        sword_summon_1.fire_once();
        sword_summon_2.fire_once();
        sword_summon_3.fire_once();
        sword_summon_4.fire_once();
        sword_summon_5.fire_once();    
    }
}
