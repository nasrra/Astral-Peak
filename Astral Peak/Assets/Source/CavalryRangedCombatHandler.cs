using System;
using UnityEngine;
using UnityEngine.Experimental.AI;

public class CavalryRangedCombatHandler : MonoBehaviour{
    public event Action<GameObject> fetch_sword_fired;
    public event Action arrow_fired;
    [SerializeField] Turret
        fetch_sword = new Turret(),
        howl_arrow  = new SmartTurret(),
        jump_arrow  = new SmartTurret(),
        ground_slam_1 = new Turret(),
        ground_slam_2 = new Turret();

    void Start() => link();
    void OnDestroy() => unlink();
    
    public void fetch_sword_fire_once() => fetch_sword.fire_once();
    
    public void ground_slams(){
        ground_slam_1.fire_once();
        ground_slam_2.fire_once();
    }
    
    public void howl_arrows() => howl_arrow.fire_once();
    public void jump_arrow_fire_once() => jump_arrow.fire_once(); 

    public void foo(GameObject x){fetch_sword_fired?.Invoke(x);}

    void bow_fired(GameObject arrow) => arrow_fired?.Invoke();

    void link(){
        fetch_sword.projectile_fired    += foo;
        howl_arrow.projectile_fired     += bow_fired;
        jump_arrow.projectile_fired     += bow_fired;
        ground_slam_1.projectile_fired  += bow_fired;
        ground_slam_2.projectile_fired  += bow_fired;
    }
    void unlink(){
        fetch_sword.projectile_fired    -= foo;
        howl_arrow.projectile_fired     -= bow_fired;
        jump_arrow.projectile_fired     -= bow_fired;
        ground_slam_1.projectile_fired  -= bow_fired;
        ground_slam_2.projectile_fired  -= bow_fired;
    }
}
