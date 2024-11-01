using System;
using UnityEngine;

public class CavalryRangedCombatHandler : MonoBehaviour{
    public event Action<GameObject> fetch_sword_fired;
    [SerializeField] Turret
        bite_3_beam,
        fetch_sword,
        sword_summon_1,
        sword_summon_2,
        sword_summon_3,
        sword_summon_4,
        sword_summon_5,
        ground_slam_1,
        ground_slam_2;

    void Start() => link();
    void OnDestroy() => unlink();
    
    public void fetch_sword_fire_once() => fetch_sword.fire_once();
    public void bite_3_beam_fire_once() => bite_3_beam.fire_once();
    
    public void ground_slams(){
        ground_slam_1.fire_once();
        ground_slam_2.fire_once();
    }
    
    public void summon_swords(){
        sword_summon_1.fire_once();
        sword_summon_2.fire_once();
        sword_summon_3.fire_once();
        sword_summon_4.fire_once();  
        sword_summon_5.fire_once();
    }

    void link() => fetch_sword.projectile_fired += foo;
    public void foo(GameObject x){fetch_sword_fired?.Invoke(x);}
    void unlink() => fetch_sword.projectile_fired -= fetch_sword_fired;
}
