using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CavalryBossRoom : BossRoomHandler{
    [SerializeField] public TheCavalry cavalry;
    [SerializeField] public TheRider rider;
    [SerializeField] Collider2DFeedback feedback;
    [SerializeField] Collider2D feedback_collider;

    void Awake(){
        link();
        instance = this;
    }

    void Start() => AudioManager.play_ambience(SoundLibrary.get_sound(SoundID.SOFT_WIND));

    void OnDisable() => unlink();

    public override void prepare_phase_transition(){
        base.prepare_phase_transition();
        cavalry.gameObject.SetActive(false);
        rider.gameObject.SetActive(false);
        switch(phase){
            case 1:  
                song = SoundID.WOLF_BOSS_MUSIC;
                prepare_scene = phase_1;
                prepare_scene.Invoke();
                play_cinematic = false;
                break;
            case 2: 
                song = SoundID.WOLF_BOSS_MUSIC;
                prepare_scene = phase_2;
                cinematic = "cavalry_transition_1";
                play_cinematic = true;
                break;
        }
    }

    void player_entered(Collider2D col){
        feedback.enabled = false;  
        feedback_collider.enabled = false;      
        phase_transition();
        play_music();
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
        cavalry.death           += phase_transition;
        rider.death             += phase_transition;
        feedback.trigger_enter  += player_entered;
    }

    void unlink(){
        cavalry.death           -= phase_transition;
        rider.death             -= phase_transition;
        feedback.trigger_enter  -= player_entered;
    }
}
