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
    protected bool play_cinematic = false;
    public SoundID song; 
    protected Cutscene cutscene; 
    public int phase = 0;
    void Awake(){
        instance = this;
        AudioManager.stop_music();
    }

    protected abstract void check_world_state();

    public void start_fight(){
        phase_transition();
        fight_started?.Invoke();
    }

    public void stop_fight() => fight_stopped?.Invoke();

    public Transform get_boss_point(int index) => boss_points[index];
    public virtual void prepare_phase_transition() => phase++;
    public void phase_transition(){
        prepare_phase_transition();
        if(play_cinematic == true)
            CutsceneManager.play(cutscene);
    }
    public void set_respawn_point() => Player.instance.set_respawn_point(respawn_point.name);    
}
