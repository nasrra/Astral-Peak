using System;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public static class CustomSceneManager{
    static string scene_to_load;
    public static Action loading_scene, loaded_scene;
    public static void load_scene(string _scene, bool _scene_transitions = true){
        scene_to_load = _scene;
        if(_scene_transitions == true)
            load_scene_with_transitions();
        else
            load_scene();
    }

    static void load_scene() => UnityHook.instance.StartCoroutine(load_scene_coroutine());
    static IEnumerator load_scene_coroutine(){
        // Wait to unload scene
        Scene active = SceneManager.GetActiveScene();
        AsyncOperation load;
        AsyncOperation unload;
        
        // load temp
        load = SceneManager.LoadSceneAsync("temp", LoadSceneMode.Additive);
        yield return load;

        // unload active
        unload = SceneManager.UnloadSceneAsync(active);
        yield return unload;

        // laod scene.
        load = SceneManager.LoadSceneAsync(scene_to_load, LoadSceneMode.Single);
        AudioManager.restore_sfx_smooth();
        yield return load;
        loaded_scene?.Invoke();
        yield break;
    }

    static void load_scene_with_transitions() => UnityHook.instance.StartCoroutine(load_scene_with_transitions_coroutine());
    static IEnumerator load_scene_with_transitions_coroutine(){
        AudioManager.dim_sfx_smooth();
        CameraEffects.instance.completed_fade_to_black += load_scene; // has to be linked beforehand to ensure the IEnumerator instance of the action isnt null.
        CameraEffects.instance.fade_to_black();
        yield break;
    } 
    static void test() => Debug.Log(1);
}
