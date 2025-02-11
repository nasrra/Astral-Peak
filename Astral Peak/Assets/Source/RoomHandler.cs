using System.Collections.Generic;
using Entropek;
using Sounds;
using UnityEngine;

public class RoomHandler : MonoBehaviour{
    public static RoomHandler instance;
    [Header("RoomHandler")]
    [SerializeField] protected List<SoundID> ambience_tracks = new List<SoundID>();   
    [SerializeField] Transform final_cutscene_camera_target;
    [SerializeField] FinalCutsceneCameraMovementOption final_cutscene_camera_movement;
    protected virtual void Awake(){
        instance = this;
    }

    void OnDestroy(){
        instance = null;
    }

    protected virtual void Start(){
        AudioManager.play_ambience(ambience_tracks[0]);
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

