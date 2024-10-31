using System;
using UnityEngine;

public class CavalryRangedCombat : MonoBehaviour{
    public event Action<GameObject> fetch_sword_fired;
    [SerializeField] Turret
        bite_3_beam_turret,
        fetch_sword_turret;

    void Start() => link();
    void OnDestroy() => unlink();
    
    public void fetch_sword_fire_once() => fetch_sword_turret.fire_once();
    public void bite_3_beam_fire_once() => bite_3_beam_turret.fire_once();

    void link() => fetch_sword_turret.projectile_fired += foo;
    public void foo(GameObject x){fetch_sword_fired?.Invoke(x);}
    void unlink() => fetch_sword_turret.projectile_fired -= fetch_sword_fired;
}
