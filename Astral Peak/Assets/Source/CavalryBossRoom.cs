using UnityEngine;
using System.Collections.Generic;
using Entropek;

public class CavalryBossRoom : BossRoomHandler{
    [field: SerializeField] public Cavalry cavalry {get; private set;}
    [field: SerializeField] public Rider rider {get; private set;}
    [field: SerializeField] public CavalryCutsceneArrow cutscene_arrow {get; private set;}
    [field: SerializeField] public GameObject background_wolf {get; private set;}
    [field: SerializeField] public ParticleSystem wolf_impact_transition {get; private set;}
    [SerializeField] Transform rider_start_point, cavalry_start_point;

    protected override void Awake(){
        link();
        cutscenes = new Dictionary<string, System.Func<Cutscene>>(){
            {"opening",()=>new Cutscenes.CavalryOpening()},
            {"phase_1",()=>new Cutscenes.CavalryPhaseTransition()}
        };
        base.Awake();
    } 

    protected override void OnDestroy(){
        unlink();
        base.OnDestroy();
    }

    protected override void check_world_state(){
        if(GameManager.get_boss_state(0)==true){
            unlink_fight_start_trigger();
            Player.instance.set_spawn_point(fight_start_trigger.gameObject.name);
            exit.set_start_open(true);
        }
    }

    protected override int get_boss_id() => 0;
    protected override Cutscene get_altar_cutscene() => new ShrineAltarOneCutscene();

    void link(){
        link_fight_start_trigger();
        cavalry.death_started   += death_started;
        cavalry.death_completed += death_completed;
        rider.phase_transition  += phase_transition;
    }

    void unlink(){
        unlink_fight_start_trigger();
        cavalry.death_started   -= death_started;
        cavalry.death_completed -= death_completed;
        rider.phase_transition  -= phase_transition;
    }
}
