using UnityEngine;
using System.Collections;

public class SceneTransitionDoor : Door{

    // Data.
    [Header("Scene Transition Door")]
    [SerializeField] private string scene_to_load, exit_point;

    public override void enter(){
        StartCoroutine(enter_coroutine());
    }    
    IEnumerator enter_coroutine(){
        Player.spawn_point = exit_point;
        Player.instance.door_enter_state();
        CameraController.instance.set_target(transform);
        CustomSceneManager.load_scene_with_transitions(scene_to_load); 
        close();
        yield break;
    }
}