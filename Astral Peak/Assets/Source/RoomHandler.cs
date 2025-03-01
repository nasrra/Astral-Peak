using System.Collections.Generic;
using UnityEngine;

public class RoomHandler : MonoBehaviour{
    public static RoomHandler instance;
    public static RoomType current_room_type = RoomType.NONE;
    [Header("RoomHandler")]
    [SerializeField] RoomType room_type;
    public string ambience_track;
    public string music_track;
    [SerializeField] Transform final_cutscene_camera_target;
    [SerializeField] FinalCutsceneCameraMovementOption final_cutscene_camera_movement;
    protected virtual void Awake(){
        if (current_room_type == RoomType.NONE)
            AudioManager.load_bank(room_type);
        else if(current_room_type != room_type){
            AudioManager.unload_bank(current_room_type);
            AudioManager.load_bank(room_type);
        }
        current_room_type = room_type;
        instance = this;
        if(ambience_track != "" && ambience_track != null)
            AudioManager.play_ambience(ambience_track);
        else
            AudioManager.stop_ambience();
        if(music_track != "" && music_track != null)
            AudioManager.play_music(music_track);
        else
            AudioManager.stop_music();
    }
    
    protected virtual void Start(){
        // Debug.Log(current_room_type+" "+room_type);
    }

    protected virtual void OnDestroy(){
        if(SceneInfo.instance.transition == true)
            AudioManager.unload_bank(room_type);
    }

    public virtual void game_cleared_room_state(){
        game_cleared_camera_movement();
        Player.instance.gameObject.SetActive(false);
        if(EnemyManager.instance != null){
            Debug.Log("called");
            EnemyManager.instance.set_start_all_inactive(true);
        }
        
    }

    private void game_cleared_camera_movement(){
        CameraController.instance.set_target(final_cutscene_camera_target);
        CameraController.instance.snap_to_target();
        if(final_cutscene_camera_movement == FinalCutsceneCameraMovementOption.LEFT)
            CameraController.instance.lerp_offset(-15,null,10);
        else if(final_cutscene_camera_movement == FinalCutsceneCameraMovementOption.DOWN)
            CameraController.instance.lerp_offset(null,-15,15);
    }

    enum FinalCutsceneCameraMovementOption{
        LEFT,DOWN,NONE
    }
}

public enum RoomType{
    SHRINE,
    SNOW,
    NONE,
}