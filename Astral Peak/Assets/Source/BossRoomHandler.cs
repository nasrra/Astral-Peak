using UnityEngine;

public class BossRoomHandler : MonoBehaviour{
    public static BossRoomHandler instance;
    public Door player_respawn_point;
    public Transform boss_start_point;
    public bool play_cinematic = true;
    public string opening_cinematic;
    public string boss_song;
    void Awake()=>instance = this;
    // play the opening cutscene to a boss room.
    void Start(){
        if(play_cinematic == true)
            CutsceneManager.instance.Play(opening_cinematic);
        MusicManager.instance.play_music(SoundLibrary.music[boss_song]());
        MusicManager.instance.play_ambience(SoundLibrary.sfx["wind"]());
    }
}
