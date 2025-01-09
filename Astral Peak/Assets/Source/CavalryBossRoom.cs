using UnityEngine;
using System.Collections;
using Sounds;

public class CavalryBossRoom : BossRoomHandler{
    [SerializeField] TheCavalry cavalry;
    [SerializeField] TheRider rider;
    [SerializeField] public CavalryCutsceneArrow cutscene_arrow;
    [SerializeField] public GameObject background_wolf;
    [SerializeField] public ParticleSystem wolf_impact_transition;
    [SerializeField] Collider2DFeedback feedback;
    [SerializeField] Collider2D feedback_collider;
    [SerializeField] Transform rider_start_point, cavalry_start_point;

    void Awake() => check_world_state();

    void Start(){
        link();
        instance = this;
        AudioManager.play_ambience(SoundID.SOFT_WIND);
    } 

    void OnDestroy(){
        unlink();
    }

    public TheCavalry get_cavalary() => cavalry;
    public TheRider get_rider() => rider;

    protected override void check_world_state(){
        if(GameManager.world_state >=1){
            feedback.enabled = false;
            feedback_collider.enabled = false;
            Player.instance.set_spawn_point(feedback.gameObject.name);
        }
    }

    public override void prepare_phase_transition(){
        base.prepare_phase_transition();
        cavalry.gameObject.SetActive(false);
        rider.gameObject.SetActive(false);
        switch(phase){
            case 1:  
                song = SoundID.WOLF_BOSS_MUSIC_1;
                cutscene = new Cutscenes.CavalryOpening();
                play_cinematic = true;
                break;
            case 2: 
                song = SoundID.WOLF_BOSS_MUSIC_2;
                cutscene = new Cutscenes.CavalryPhaseTransition();
                play_cinematic = true;
                break;
        }
    }

    void player_entered(Collider2D col){//
        feedback.enabled = false;  
        feedback_collider.enabled = false;      
        start_fight();
    }

    public void phase_1(){
        //boss_start_point = rider_start_point;
        cavalry.gameObject.SetActive(false);
        rider.gameObject.SetActive(true);
    }

    public void phase_2(){
        //boss_start_point = cavalry_start_point;
        cavalry.gameObject.SetActive(true);
        rider.gameObject.SetActive(false);   
    }

    public void set_positions(){
        //rider.transform.position = boss_start_point.position;
        //cavalry.transform.position = boss_start_point.position;
        Player.instance.set_enter_position();
    }

    void fight_ended() => StartCoroutine(altar_cutscene());
    IEnumerator altar_cutscene(){
        yield return new WaitForSeconds(6);
        CustomSceneManager.load_scene("Shrine");
        CustomSceneManager.loaded_scene += play_altar_cutscene;
        Player.instance.enter_cutscene_state();
        yield break;
    }

    void play_altar_cutscene(){
        CutsceneManager.play(new ShrineAltarOneCutscene());
        CustomSceneManager.loaded_scene -= play_altar_cutscene;
    }

    void link(){
        cavalry.death_completed += fight_ended;
        rider.death_completed   += phase_transition;
        feedback.trigger_enter  += player_entered;
    }

    void unlink(){
        cavalry.death_completed -= fight_ended;
        rider.death_completed   -= phase_transition;
        feedback.trigger_enter  -= player_entered;
    }
}
