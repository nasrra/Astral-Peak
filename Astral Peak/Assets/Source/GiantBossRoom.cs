using System;
using System.Collections.Generic;
using Entropek;
using UnityEngine;

public class GiantBossRoom : BossRoomHandler{

    [field: SerializeField] public Giant1 giant1 {get ; private set;}
    [SerializeField] Giant2 giant2;
    [SerializeField] DomineDoor domine_door_script;
    [SerializeField] ConstellationController gateway_1, gateway_2;
    [SerializeField] Transform domine_door_transform;

    protected override void Awake(){
        link_events();
        cutscenes = new Dictionary<string, Func<Cutscene>>(){
            {"opening",()=>new Cutscenes.GiantOpening()},
            {"phase_1",()=>throw new Exception("no phase_1 cutscene for giant boss!")}
        };
        base.Awake();
    }

    protected override void check_world_state(){
        if(GameManager.get_boss_state(2)==true){
            unlink_fight_start_trigger();
            Player.instance.set_spawn_point(respawn_points[0].gameObject.name);
            CutsceneManager.play(new Cutscenes.DomineDoorOpening());
        }
    }
    protected override Cutscene get_altar_cutscene() => new ShrineAltarThreeCutscene();
    protected override int get_boss_id() => 2;

    public DomineDoor get_domine_door() => domine_door_script;
    public ConstellationController get_gateway_1() => gateway_1;
    public ConstellationController get_gateway_2() => gateway_2;

    void link_events(){
        link_mage();
        link_fight_start_trigger();
    }
    void unlink_events(){
        unlink_mage();
        unlink_fight_start_trigger();
    }

    public void enable_giant2(){
        giant2.gameObject.SetActive(true);
        giant2.play_intro_animation();
    }

    void link_mage(){
        giant1.death_completed     += enable_giant2;
        giant2.death_started       += death_started;
        giant2.death_completed     += death_completed;        
        giant2.death_completed     += unlink_events;        
    }
    void unlink_mage(){
        giant1.death_completed     -= enable_giant2;
        giant2.death_started       -= death_started;
        giant2.death_completed     -= death_completed;        
        giant2.death_completed     -= unlink_events; 
    }
}
