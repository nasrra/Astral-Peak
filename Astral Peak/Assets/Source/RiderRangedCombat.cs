using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiderRangedCombat : MonoBehaviour{
    public event Action arrow_fired;
    [SerializeField] Turret 
        signature_arrow = new SmartTurret(),
        back_shot_arrow = new SmartTurret();
    [SerializeField]
        List<SmartTurret> round_shot_arrows = new List<SmartTurret>();

    void Start  () => link();
    void OnDestroy() => unlink();

    public void fire_signature_arrow()          => signature_arrow.fire_once();
    public void fire_back_shot_arrow()          => back_shot_arrow.fire_once();
    public void fire_round_shot_arrow(int x)    => round_shot_arrows[x].fire_once();
    void bow_shot(GameObject x) => arrow_fired.Invoke();
    void link(){
        signature_arrow.projectile_fired += bow_shot;
        back_shot_arrow.projectile_fired += bow_shot;
        foreach(SmartTurret s in round_shot_arrows)
            s.projectile_fired += bow_shot;
    }
    void unlink(){
        signature_arrow.projectile_fired -= bow_shot;
        back_shot_arrow.projectile_fired -= bow_shot;
        foreach(SmartTurret s in round_shot_arrows)
            s.projectile_fired -= bow_shot;
    }  
}
