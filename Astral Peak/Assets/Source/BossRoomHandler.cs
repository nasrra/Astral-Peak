using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public abstract class BossRoomHandler : RoomHandler{
    public event Action 
        fight_started,
        fight_stopped;
    [Header("BossRoomHandler")]
    [SerializeField] protected List<Transform> boss_points = new List<Transform>(); 
    [SerializeField] protected List<Transform> respawn_points = new List<Transform>();
    [SerializeField] protected Collider2DFeedback fight_start_trigger;
    [SerializeField] protected Door exit;
    protected Dictionary<string, Func<Cutscene>> cutscenes;
    protected Cutscene cutscene; 
    protected override void Awake(){
        base.Awake();
        // AudioManager.stop_music();
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
        Player.instance.transform.position = fight_start_trigger.transform.position;
        Player.instance.get_movement().zero_velocity();
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

    protected void phase_transition(string phase){
        ProjectileManager.instance?.destroy_all();
        EnemyManager.instance?.destroy_all();
        play_cutscene(phase);
    }

    protected virtual void death_started(){
        ProjectileManager.instance?.destroy_all();
        EnemyManager.instance?.destroy_all();
        GameManager.boss_defeated(get_boss_id());
        GameManager.invoke_set_game_data();
        GameManager.save_game_data();
    }

    protected virtual void death_completed(){
        UiManager.instance.play_enemy_vanquished();
        // AudioManager.stop_music();
        StartCoroutine(altar_cutscene());
    }
    protected IEnumerator altar_cutscene(){
        yield return new WaitForSeconds(6);
        AudioManager.stop_ambience();
        CutsceneManager.play(get_altar_cutscene());
        yield break;
    }

    protected abstract int get_boss_id();
    protected abstract Cutscene get_altar_cutscene();
}//
