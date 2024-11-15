using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CavalryBossRoom : BossRoomHandler{
    [SerializeField] TheCavalry cavalry;
    [SerializeField] TheRider rider;

    void Awake() => link();
    void OnDisable() => unlink();

    protected override void prepare_phase_transition(){
        base.prepare_phase_transition();
        cavalry.gameObject.SetActive(false);
        rider.gameObject.SetActive(false);
        switch(phase){
            case 1: 
                cavalry.gameObject.SetActive(true); 
                song = "boss1";
                break;
            case 2: 
                rider.gameObject.SetActive(true); 
                song = "boss1";
                break;
        }
    }

    void link(){
        cavalry.death   += phase_transition;
        rider.death     += phase_transition;
    }

    void unlink(){
        cavalry.death   -= phase_transition;
        rider.death     -= phase_transition;
    }
}
