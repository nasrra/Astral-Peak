using System.Collections.Generic;
using UnityEngine;

public class BossRoomHandler : MonoBehaviour{
    public static BossRoomHandler instance;
    public Door player_respawn_point;
    public Transform boss_start_point;
    public string song, cinematic;
    public int phase = 0;
    void Awake() => instance = this;
    
    // play the opening cutscene to a boss room.
    void Start(){
        AudioManager.play_ambience(SoundLibrary.sfx["wind"]());
        phase_transition();
    }

    protected virtual void prepare_phase_transition(){
        phase++;
    }

    public void phase_transition(){
        prepare_phase_transition();
        AudioManager.play_music(SoundLibrary.music[song]());
        //CutsceneManager.instance.Play(cinematics[phase]);
    }
}
