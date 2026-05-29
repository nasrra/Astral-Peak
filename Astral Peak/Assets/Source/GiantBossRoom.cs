using System;
using System.Collections;
using System.Collections.Generic;
using Entropek;
using Unity.VisualScripting;
using UnityEngine;

public class GiantBossRoom : BossRoomHandler{

    [field: SerializeField] public Giant1 giant1 {get ; private set;}
    [SerializeField] Giant2 giant2;
    [SerializeField] Giant2Hand left_hand, right_hand;
    [SerializeField] DomineDoor domine_door_script;
    [SerializeField] ConstellationController gateway_1, gateway_2;
    [SerializeField] Transform domine_door_transform;
    [SerializeField] Torch[] torches;

    protected override void Awake(){
        link_events();
        cutscenes = new Dictionary<string, Func<Cutscene>>(){
            {"opening",()=>new Cutscenes.GiantOpening()},
            {"phase_1",()=>throw new Exception("no phase_1 cutscene for giant boss!")}
        };
        base.Awake();
    }

    protected override void check_world_state(){
        if(GameManager.get_boss_state(2)==true && GameManager.get_state() != GameState.CUTSCENE){
            unlink_fight_start_trigger();
            Player.instance.respawn_point = respawn_points[0].gameObject.name;
            CutsceneManager.play(new Cutscenes.DomineDoorOpening());
            disable_arena_bounds();
            turn_on_torches_instant();
        }
        else{
            enable_arena_bounds();
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


    private void turn_on_torches_instant(){
        for(int i = 0; i < torches.Length; i++){
            torches[i].on();    
        }
    }
    private void turn_on_torches(){
        StartCoroutine(turn_on_torches_coroutine());
    }
    IEnumerator turn_on_torches_coroutine(){
        for(int i = 0; i < torches.Length/2; i++){
            torches[i].turn_on();
            torches[torches.Length - i - 1].turn_on();
            yield return new WaitForSeconds(1f);
        }
        yield break;
    }

    public void enable_giant2(){
        set_room_state(1);
        turn_on_torches();
        giant2.gameObject.SetActive(true);
        left_hand.gameObject.SetActive(true);
        right_hand.gameObject.SetActive(true);
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
