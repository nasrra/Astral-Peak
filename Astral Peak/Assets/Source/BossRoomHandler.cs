using System.Collections.Generic;
using UnityEngine;

public class BossRoomHandler : MonoBehaviour{
    public delegate void ScenePreperation();
    public ScenePreperation prepare_scene;
    public static BossRoomHandler instance;
    public Door player_respawn_point;
    public Transform boss_start_point;
    public bool play_cinematic = false;
    public string song, cinematic;
    public int phase = 0;
    void Awake() => instance = this;
    
    // play the opening cutscene to a boss room.
    void Start(){
        AudioManager.play_ambience(SoundLibrary.sfx["wind"]());
        phase_transition();
        play_music();
    }

    public virtual void prepare_phase_transition() => phase++;
    protected void play_music()     => AudioManager.play_music(SoundLibrary.music[song]());
    public void phase_transition(){
        prepare_phase_transition();
        if(play_cinematic == true)
            CutsceneManager.play(cinematic);
    }
}
