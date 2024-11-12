using UnityEngine;

public class BossRoomManager : MonoBehaviour{
    public static BossRoomManager instance;
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
        AudioManager.Play(boss_song);
        //AudioManager.music_volume(-40);
    }
}
