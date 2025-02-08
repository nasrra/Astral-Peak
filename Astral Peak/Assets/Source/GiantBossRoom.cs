using System;
using System.Collections.Generic;
using Entropek;
using UnityEngine;

public class GiantBossRoom : BossRoomHandler{

    [SerializeField] Giant1 giant1_script;
    [SerializeField] Giant2 giant2_script;
    [SerializeField] GameObject giant1_object, giant_2_object;

    protected override void Awake(){
        link_events();
        cutscenes = new Dictionary<string, Func<Cutscene>>(){
            {"opening",()=>new Cutscenes.GiantOpening()},
            {"phase_1",()=>new Cutscenes.GiantPhaseTransition()}
        };
        base.Awake();
    }

    protected override void check_world_state(){
        if(GameManager.get_boss_state(2)==true){
            Log.MethodCall();
            unlink_fight_start_trigger();
            Player.instance.set_spawn_point(fight_start_trigger.gameObject.name);
            //exit.set_start_open(true);
        }
    }
    protected override Cutscene get_altar_cutscene() => new ShrineAltarThreeCutscene();
    protected override int get_boss_id() => 2;

    void link_events(){
        link_mage();
        link_fight_start_trigger();
    }
    void unlink_events(){
        unlink_mage();
        unlink_fight_start_trigger();
    }
    void link_mage(){
        giant1_script.phase_transition    += play_cutscene;
        giant2_script.death_started       += death_started;
        giant2_script.death_completed     += death_completed;        
        giant2_script.death_completed     += unlink_events;        
    }
    void unlink_mage(){
        giant1_script.phase_transition    -= play_cutscene;
        giant2_script.death_started       -= death_started;
        giant2_script.death_completed     -= death_completed;        
        giant2_script.death_completed     -= unlink_events; 
    }
}
