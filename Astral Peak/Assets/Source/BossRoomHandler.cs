using System.Collections.Generic;
using UnityEngine;

public class BossRoomHandler : MonoBehaviour{
    public static BossRoomHandler instance;
    public Door player_respawn_point;
    public Transform boss_start_point;
    public bool play_cinematic = false;
    public SoundID song; 
    public string cinematic;
    public int phase = 0;
    void Awake() => instance = this;

    public virtual void prepare_phase_transition() => phase++;
    protected void play_music() => AudioManager.play_music(SoundLibrary.get_sound(song));
    public void phase_transition(){
        prepare_phase_transition();
        if(play_cinematic == true)
            CutsceneManager.play(cinematic);
    }
}
