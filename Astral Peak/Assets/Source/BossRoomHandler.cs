using System;
using UnityEngine;
using Sounds;
using System.Collections.Generic;

public abstract class BossRoomHandler : MonoBehaviour{
    public event Action 
        fight_started,
        fight_stopped;
    public static BossRoomHandler instance;
    [SerializeField] protected List<Transform> boss_points = new List<Transform>(); 
    [SerializeField] protected Transform respawn_point;
    [SerializeField] protected Collider2DFeedback cutscene_trigger;
    [SerializeField] protected Door exit;
    protected Dictionary<string, Func<Cutscene>> cutscenes;
    protected bool play_cinematic = false;
    protected Cutscene cutscene; 
    public int phase = 0;
    protected virtual void Awake(){
        instance = this;
//        AudioManager.stop_music();
        check_world_state();
    }

    protected abstract void check_world_state();

    public void start_fight(){
        fight_started?.Invoke();
    }

    public void stop_fight() => fight_stopped?.Invoke();

    public Transform get_boss_point(int index) => boss_points[index];
    protected void play_cutscene(string phase) => CutsceneManager.play(cutscenes[phase]());
    public void set_respawn_point() => Player.instance.set_respawn_point(respawn_point.name);    
}
