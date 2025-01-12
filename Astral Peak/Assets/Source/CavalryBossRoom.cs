using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Sounds;
using Entropek;

public class CavalryBossRoom : BossRoomHandler{
    [SerializeField] Cavalry cavalry;
    [SerializeField] Rider rider;
    [SerializeField] public CavalryCutsceneArrow cutscene_arrow;
    [SerializeField] public GameObject background_wolf;
    [SerializeField] public ParticleSystem wolf_impact_transition;
    [SerializeField] Transform rider_start_point, cavalry_start_point;

    protected override void Awake(){
        link();
        cutscenes = new Dictionary<string, System.Func<Cutscene>>(){
            {"opening",()=>new Cutscenes.CavalryOpening()},
            {"phase_1",()=>new Cutscenes.CavalryPhaseTransition()}
        };
        base.Awake();
    } 

    void Start()=>AudioManager.play_ambience(SoundID.SOFT_WIND);
    void OnDestroy(){
        unlink();
    }

    public Cavalry get_cavalary() => cavalry;
    public Rider get_rider() => rider;

    protected override void check_world_state(){
        if(GameManager.get_boss_state(0)==true){
            Log.MethodCall();
            unlink_fight_start_trigger();
            Player.instance.set_spawn_point(fight_start_trigger.gameObject.name);
            exit.set_start_open(true);
        }
    }

    public void phase_1(){
        //boss_start_point = rider_start_point;
        cavalry.gameObject.SetActive(false);
        rider.gameObject.SetActive(true);
    }

    public void set_positions(){
        //rider.transform.position = boss_start_point.position;
        //cavalry.transform.position = boss_start_point.position;
        Player.instance.set_enter_position();
    }

    void death_started(){
        ProjectileManager.instance.destroy_all();
        GameManager.set_boss_state(0,true);
    }
    void death_completed() => StartCoroutine(altar_cutscene());
    IEnumerator altar_cutscene(){
        yield return new WaitForSeconds(6);
        CustomSceneManager.load_scene("Shrine");
        CustomSceneManager.loaded_scene += play_altar_cutscene;
        Player.instance.enter_cutscene_state();
        yield break;
    }

    void phase_transition(string phase){
        ProjectileManager.instance.destroy_all();
        play_cutscene(phase);
    }

    void play_altar_cutscene(){
        CutsceneManager.play(new ShrineAltarOneCutscene());
        CustomSceneManager.loaded_scene -= play_altar_cutscene;
    }

    void link(){
        Log.MethodCall();
        link_fight_start_trigger();
        cavalry.death_started   += death_started;
        cavalry.death_completed += death_completed;
        rider.phase_transition  += play_cutscene;
    }

    void unlink(){
        unlink_fight_start_trigger();
        cavalry.death_started   -= death_started;
        cavalry.death_completed -= death_completed;
        rider.phase_transition  -= play_cutscene;
    }
}
