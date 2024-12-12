using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;

public class SceneTransitionDoor : Door{

    // Data.
    [Header("Scene Transition Door")]
    [SerializeField] private string scene_to_load, exit_point;

    public override void enter() => StartCoroutine(enter_coroutine());
    IEnumerator enter_coroutine(){
        Player.instance.set_spawn_point(exit_point);
        Player.instance.door_enter_state();
        CustomSceneManager.load_scene(scene_to_load); 
        close();
        yield break;
    }
}