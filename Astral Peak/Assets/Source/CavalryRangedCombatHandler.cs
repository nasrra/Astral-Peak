using System;
using UnityEngine;

public class CavalryRangedCombatHandler : MonoBehaviour{
    public event Action<GameObject> fetch_sword_fired;
    [SerializeField] Turret
        bite_3_beam_turret,
        fetch_sword_turret,
        sword_summon_turret_1,
        sword_summon_turret_2,
        sword_summon_turret_3,
        sword_summon_turret_4,
        sword_summon_turret_5;

    void Start() => link();
    void OnDestroy() => unlink();
    
    public void fetch_sword_fire_once() => fetch_sword_turret.fire_once();
    public void bite_3_beam_fire_once() => bite_3_beam_turret.fire_once();
    public void summon_swords(){
        sword_summon_turret_1.fire_once();
        sword_summon_turret_2.fire_once();
        sword_summon_turret_3.fire_once();
        sword_summon_turret_4.fire_once();  
        sword_summon_turret_5.fire_once();
    }

    void link() => fetch_sword_turret.projectile_fired += foo;
    public void foo(GameObject x){fetch_sword_fired?.Invoke(x);}
    void unlink() => fetch_sword_turret.projectile_fired -= fetch_sword_fired;
}
