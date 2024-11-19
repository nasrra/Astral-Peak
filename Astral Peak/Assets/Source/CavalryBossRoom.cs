using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CavalryBossRoom : BossRoomHandler{
    [SerializeField] TheCavalry cavalry;
    [SerializeField] TheRider rider;
    [SerializeField] public GameObject background_wolf;
    [SerializeField] public Turret cutscene_arrow;
    [SerializeField] public ParticleSystem wolf_impact_transition;
    [SerializeField] Collider2DFeedback feedback;
    [SerializeField] Collider2D feedback_collider;
    [SerializeField] Transform rider_start_point, cavalry_start_point;

    void Awake(){
        link();
        instance = this;
        cutscene_arrow.projectile_fired += play_arrow_shot;
    }

    [SerializeField] SmartTurret turret;
    AudioSource source;

    void Start(){
        AudioManager.play_ambience(SoundID.SOFT_WIND);
    } 

    void OnDestroy(){
        cutscene_arrow.projectile_fired += play_arrow_shot;
        unlink();
    }

    public override void prepare_phase_transition(){
        base.prepare_phase_transition();
        cavalry.gameObject.SetActive(false);
        rider.gameObject.SetActive(false);
        switch(phase){
            case 1:  
                song = SoundID.WOLF_BOSS_MUSIC_1;
                cinematic = "cavalry_opening";
                play_cinematic = true;
                break;
            case 2: 
                song = SoundID.WOLF_BOSS_MUSIC_2;
                cinematic = "cavalry_transition_1";
                play_cinematic = true;
                break;
        }
    }

    void player_entered(Collider2D col){
        feedback.enabled = false;  
        feedback_collider.enabled = false;      
        Player.player.set_exit_point(player_respawn_point.get_enter_point());
        phase_transition();
    }

    public void phase_1(){
        boss_start_point = rider_start_point;
        cavalry.gameObject.SetActive(false);
        rider.gameObject.SetActive(true);
    }

    public void phase_2(){
        boss_start_point = cavalry_start_point;
        cavalry.gameObject.SetActive(true);
        rider.gameObject.SetActive(false);   
    }

    public void set_positions(){
        rider.transform.position = boss_start_point.position;
        cavalry.transform.position = boss_start_point.position;
        Player.player.set_enter_position();
    }

    void play_arrow_shot(GameObject x) => AudioClipHandler.play(this, SoundID.BOW_SHOT, out source); 

    void link(){
        //cavalry.death           += phase_transition;
        rider.death             += phase_transition;
        feedback.trigger_enter  += player_entered;
    }

    void unlink(){
        //cavalry.death           -= phase_transition;
        rider.death             -= phase_transition;
        feedback.trigger_enter  -= player_entered;
    }
}
