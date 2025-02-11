using System;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections.Generic;

public static class CustomSceneManager{
    static string scene_to_load;
    public static Action loading_scene, loaded_scene;

    static readonly HashSet<string> dont_save_scenes = new HashSet<string> {
        "MainMenu", "SplashScreen", "Credits", "DemoEnd", "Introduction", "AstralPlane"
    };

    public static void initialize(){
        GameManager.set_game_data += set_game_data;
    }

    public static void uninitialize(){
        GameManager.set_game_data -= set_game_data;
    }

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


    static void load_scene(){
        UnityHook.instance.StartCoroutine(load_scene_coroutine());
    }
    static IEnumerator load_scene_coroutine(){
        // Wait to unload scene
        Scene active = SceneManager.GetActiveScene();
        AsyncOperation load;
        AsyncOperation unload;
        
        // load temp
        load = SceneManager.LoadSceneAsync("temp", LoadSceneMode.Additive);
        loading_scene?.Invoke();
        yield return load;
        
        unload = SceneManager.UnloadSceneAsync(active);
        yield return unload;

        // laod scene.
        load = SceneManager.LoadSceneAsync(scene_to_load, LoadSceneMode.Single);
        AudioManager.restore_sfx_volume();
        yield return load;
        // add the title card scene here when necessary :)
        loaded_scene?.Invoke();
        if(dont_save_scenes.Contains(scene_to_load) == false){
            if(CutsceneManager.in_cutscene() == false)
                GameManager.state_changed(GameState.GAMEPLAY);
            GameManager.invoke_set_game_data();
            GameManager.save_game_data();
        }
        yield break;
    }

    static IEnumerator load_scene_with_transitions_coroutine(){
        AudioManager.dim_sfx_volume();
        if(CameraEffects.instance != null){
            CameraEffects.instance.completed_fade_to_black += load_scene; // has to be linked beforehand to ensure the IEnumerator instance of the action isnt null.
            CameraEffects.instance?.fade_to_black(1);
        }
        else
            load_scene();
        yield break;
    } 
    static IEnumerator load_scene_with_transitions_unscaled_coroutine(){
        AudioManager.dim_sfx_volume();
        if(CameraEffects.instance != null){
            CameraEffects.instance.completed_fade_to_black += load_scene; // has to be linked beforehand to ensure the IEnumerator instance of the action isnt null.
            CameraEffects.instance?.fade_to_black_unscaled(1);
        }
        else
            load_scene();
        yield break;
    } 

    static void set_game_data(){
        if(dont_save_scenes.Contains(scene_to_load) == true)
            return;
        GameManager.get_game_data().scene_to_load = scene_to_load;
    }
}
