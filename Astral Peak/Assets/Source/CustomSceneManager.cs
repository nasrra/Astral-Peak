using System;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public static class CustomSceneManager{
    static string scene_to_load;
    public static Action loading_scene, loaded_scene;
    public static void load_scene(string _scene){
        scene_to_load = _scene;
        load_scene();
    }
    public static void load_scene_with_transitions(string _scene){
        scene_to_load = _scene;
        UnityHook.instance.StartCoroutine(load_scene_with_transitions_coroutine());
    } 
    public static void load_scene_with_transitions_unscaled(string _scene){
        scene_to_load = _scene;
        UnityHook.instance.StartCoroutine(load_scene_with_transitions_unscaled_coroutine());
    }

    static void load_scene() => UnityHook.instance.StartCoroutine(load_scene_coroutine());
    static IEnumerator load_scene_coroutine(){
        // Wait to unload scene
        Scene active = SceneManager.GetActiveScene();
        AsyncOperation load;
        AsyncOperation unload;
        
        // load temp
        load = SceneManager.LoadSceneAsync("temp", LoadSceneMode.Additive);
        loading_scene?.Invoke();
        yield return load;

        // unload active
        unload = SceneManager.UnloadSceneAsync(active);
        yield return unload;

        // laod scene.
        load = SceneManager.LoadSceneAsync(scene_to_load, LoadSceneMode.Single);
        AudioManager.restore_sfx_smooth();
        yield return load;
        // add the title card scene here when necessary :)
        if(scene_to_load != "MainMenu")
            GameManager.state_changed(GameState.GAMEPLAY);
        loaded_scene?.Invoke();
        yield break;
    }

    static IEnumerator load_scene_with_transitions_coroutine(){
        AudioManager.dim_sfx_smooth();
        if(CameraEffects.instance != null){
            CameraEffects.instance.completed_fade_to_black += load_scene; // has to be linked beforehand to ensure the IEnumerator instance of the action isnt null.
            CameraEffects.instance?.fade_to_black(1);
        }
        else
            load_scene();
        yield break;
    } 
    static IEnumerator load_scene_with_transitions_unscaled_coroutine(){
        AudioManager.dim_sfx_smooth();
        if(CameraEffects.instance != null){
            CameraEffects.instance.completed_fade_to_black += load_scene; // has to be linked beforehand to ensure the IEnumerator instance of the action isnt null.
            CameraEffects.instance?.fade_to_black_unscaled(1);
        }
        else
            load_scene();
        yield break;
    } 
    static void test() => Debug.Log(1);
}
