using System;
using UnityEngine;

public class BossRoomHandler : MonoBehaviour{
    public event Action 
        fight_started,
        fight_stopped;
    public static BossRoomHandler instance;
    public SpawnPoint player_respawn_point;
    public Transform boss_start_point;
    public bool play_cinematic = false;
    public SoundID song; 
    public string cinematic;
    public int phase = 0;
    void Awake(){
        instance = this;
        AudioManager.stop_music();
    }

    public void start_fight(){
        phase_transition();
        fight_started?.Invoke();
    }

    public void stop_fight() => fight_stopped?.Invoke();

    public virtual void prepare_phase_transition() => phase++;
    public void phase_transition(){
        prepare_phase_transition();
        if(play_cinematic == true)
            CutsceneManager.play(cinematic);
    }
}
