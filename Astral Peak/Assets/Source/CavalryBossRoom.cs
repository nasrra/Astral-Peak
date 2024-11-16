using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CavalryBossRoom : BossRoomHandler{
    [SerializeField] public TheCavalry cavalry;
    [SerializeField] public TheRider rider;

    void Awake(){
        link();
        instance = this;
    }
    void OnDisable() => unlink();

    public override void prepare_phase_transition(){
        base.prepare_phase_transition();
        cavalry.gameObject.SetActive(false);
        rider.gameObject.SetActive(false);
        switch(phase){
            case 1:  
                song = "boss1";
                prepare_scene = phase_1;
                prepare_scene.Invoke();
                play_cinematic = false;
                break;
            case 2: 
                song = "boss1";
                prepare_scene = phase_2;
                cinematic = "cavalry_transition_1";
                play_cinematic = true;
                break;
        }
    }

    public void phase_1(){
        cavalry.gameObject.SetActive(false);
        rider.gameObject.SetActive(true);
    }

    public void phase_2(){
        cavalry.gameObject.SetActive(true);
        rider.gameObject.SetActive(false);   
        Player.exit_point = player_respawn_point.get_enter_point();
        Player.player.set_enter_position();     
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
