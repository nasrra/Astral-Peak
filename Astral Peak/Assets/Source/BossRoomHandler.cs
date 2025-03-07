using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public abstract class BossRoomHandler : RoomHandler{
    public event Action 
        fight_started,
        fight_stopped;
    [Header("BossRoomHandler")]
    [SerializeField] protected List<FogController> fog_controllers = new List<FogController>();
    [SerializeField] protected List<Transform> boss_points = new List<Transform>(); 
    [SerializeField] protected List<Transform> respawn_points = new List<Transform>();
    [SerializeField] protected SnowController snow_controller;
    [SerializeField] protected Collider2DFeedback fight_start_trigger;
    [SerializeField] protected Door exit;
    [SerializeField] protected int room_state = 0;
    protected Dictionary<string, Func<Cutscene>> cutscenes;
    protected Cutscene cutscene; 
    protected override void Awake(){
        // AudioManager.stop_music();
        base.Awake();
        check_world_state();
    }

    protected override void Start(){
        base.Start();
        set_room_state(room_state);
    }

    protected abstract void check_world_state();

    public void start_fight(){
        fight_started?.Invoke();
    }

    public void stop_fight() => fight_stopped?.Invoke();

    public Transform get_boss_point(int index) => boss_points[index];
    protected void play_cutscene(string phase) => CutsceneManager.play(cutscenes[phase]());
    public void set_respawn_point(int index) => Player.instance.respawn_point = respawn_points[index].name;    
    protected void start_fight(Collider2D other){
        set_respawn_point(0);//
        Player.instance.transform.position = fight_start_trigger.transform.position;
        Player.instance.get_movement().zero_velocity();
        unlink_fight_start_trigger();
        play_cutscene("phase_1");
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
        CameraController.instance.reset_offset(1);
        CameraController.instance.reset_zoom(1);
        CameraController.instance.set_target(Player.instance.transform);
    }

    protected virtual void death_completed(){
        UiManager.instance.play_enemy_vanquished();
        AudioManager.stop_music();
        StartCoroutine(altar_cutscene());
        set_room_state(0);
    }
    protected IEnumerator altar_cutscene(){
        yield return new WaitForSeconds(6);
        AudioManager.stop_ambience();
        CutsceneManager.play(get_altar_cutscene());
        yield break;
    }

    public virtual void set_room_state(int x){
        room_state = x;
        foreach(FogController fog in fog_controllers)
            fog.lerp_preset(x,4);
        snow_controller.lerp_preset(x);
        AudioManager.set_ambience_parameter("intensity",x);
    }

    protected abstract int get_boss_id();
    protected abstract Cutscene get_altar_cutscene();
}//
