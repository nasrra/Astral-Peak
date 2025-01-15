using System;
using UnityEngine;
using System.Collections.Generic;
using NUnit.Framework;

public abstract class BossRoomHandler : MonoBehaviour{
    public event Action 
        fight_started,
        fight_stopped;
    public static BossRoomHandler instance;
    [SerializeField] protected List<Transform> boss_points = new List<Transform>(); 
    [SerializeField] protected List<Transform> respawn_points = new List<Transform>();
    [SerializeField] protected Collider2DFeedback fight_start_trigger;
    [SerializeField] protected Door exit;
    protected Dictionary<string, Func<Cutscene>> cutscenes;
    protected Cutscene cutscene; 
    protected virtual void Awake(){
        instance = this;
        AudioManager.stop_music();
        check_world_state();
    }

    protected abstract void check_world_state();

    public void start_fight(){
        fight_started?.Invoke();
    }

    public void stop_fight() => fight_stopped?.Invoke();

    public Transform get_boss_point(int index) => boss_points[index];
    protected void play_cutscene(string phase) => CutsceneManager.play(cutscenes[phase]());
    public void set_respawn_point(int index) => Player.instance.set_respawn_point(respawn_points[index].name);    
    protected void start_fight(Collider2D other){
        set_respawn_point(0);//
        Player.instance.get_movement().halt();
        Player.instance.transform.position = fight_start_trigger.transform.position;
        unlink_fight_start_trigger();
        play_cutscene("opening");
    }
    protected void link_fight_start_trigger(){
        fight_start_trigger.trigger_enter += start_fight;
    }
    protected void unlink_fight_start_trigger(){//
        if(fight_start_trigger != null)
            fight_start_trigger.enabled = false;
        fight_start_trigger.trigger_enter -= start_fight;
    }
}
