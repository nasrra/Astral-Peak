using System;
using System.Collections.Generic;
using Entropek;
using UnityEngine;

public class GiantBossRoom : BossRoomHandler{

    [SerializeField] Giant1 giant1_script;
    [SerializeField] Giant2 giant2_script;
    [SerializeField] DomineDoor domine_door_script;
    [SerializeField] ConstellationController gateway_1, gateway_2;
    [SerializeField] GameObject giant1_object, giant2_object;
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
    public void play_giant1_introduction(){
        CameraController.instance.set_target(giant1_object.transform);
        giant1_object.SetActive(true);
        giant1_script.animator.Play("Giant1Intro");
        StartCoroutine(Util.timer(2, time_out:()=>giant1_script.animator.Play("Giant1Yell")));
    }

    void link_events(){
        link_mage();
        link_fight_start_trigger();
    }
    void unlink_events(){
        unlink_mage();
        unlink_fight_start_trigger();
    }

    void enable_giant2(){
        giant2_object.SetActive(true);
        giant2_script.play_intro_animation();
    }

    void link_mage(){
        giant1_script.death_completed     += enable_giant2;
        giant2_script.death_started       += death_started;
        giant2_script.death_completed     += death_completed;        
        giant2_script.death_completed     += unlink_events;        
    }
    void unlink_mage(){
        giant1_script.death_completed     -= enable_giant2;
        giant2_script.death_started       -= death_started;
        giant2_script.death_completed     -= death_completed;        
        giant2_script.death_completed     -= unlink_events; 
    }
}
