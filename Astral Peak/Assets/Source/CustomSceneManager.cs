using System;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public static class CustomSceneManager{
    static string scene_to_load;
    public static event Action preparing_scene_load, unloading_scene, loaded_scene, transitioning_scene, temp_scene;
    public static event Action<string> loading_scene, unloaded_scene;

    public static void initialize(){
        GameManager.set_game_data += set_game_data;
    }

    public static void uninitialize(){
        GameManager.set_game_data -= set_game_data;
    }

    public static void load_scene(string _scene){
        prepare_for_scene_load(_scene);
        load_scene();
    }

    public static void load_scene_with_transitions(string _scene){
        prepare_for_scene_load(_scene);
        UnityHook.instance.StartCoroutine(load_scene_with_transitions_coroutine());
    } 
    public static void load_scene_with_transitions_unscaled(string _scene){
        prepare_for_scene_load(_scene);
        UnityHook.instance.StartCoroutine(load_scene_with_transitions_unscaled_coroutine());
    }

    static void prepare_for_scene_load(string _scene){
        InputManager.disable_ui_event_system_input();
        scene_to_load = _scene;
        AudioManager.dim_sfx_audio();
        if(SceneInfo.instance.transition == true)
            transitioning_scene?.Invoke();
    }

    static void load_scene(){
        UnityHook.instance.StartCoroutine(load_scene_coroutine());
    }
    static IEnumerator load_scene_coroutine(){
        // Wait to unload scene
        Scene active = SceneManager.GetActiveScene();
        string previous_scene = active.name;
        AsyncOperation load;
        AsyncOperation unload;
        
        // load temp
        load = SceneManager.LoadSceneAsync("temp", LoadSceneMode.Additive);
        preparing_scene_load?.Invoke();
        yield return load;
        
        unloading_scene?.Invoke();
        unload = SceneManager.UnloadSceneAsync(active);
        yield return unload;

        // load scene.
        temp_scene?.Invoke(); //now in temp scene.
        unloaded_scene?.Invoke(previous_scene);
        loading_scene?.Invoke(scene_to_load);
        load = SceneManager.LoadSceneAsync(scene_to_load, LoadSceneMode.Single);
        yield return load;

        loaded_scene?.Invoke();
        if(SceneInfo.instance.saveable == true){
            if(CutsceneManager.in_cutscene() == false)
                GameManager.swap_state(GameState.GAMEPLAY);
            GameManager.invoke_set_game_data();
            GameManager.save_game_data();
        }
        AudioManager.restore_sfx_audio();
        yield break;
    }

    static IEnumerator load_scene_with_transitions_coroutine(){
        if(CameraEffects.instance != null){
            CameraEffects.instance.completed_fade_to_black += load_scene; // has to be linked beforehand to ensure the IEnumerator instance of the action isnt null.
            CameraEffects.instance?.fade_to_black(1);
        }
        else
            load_scene();
        yield break;
    } 
    static IEnumerator load_scene_with_transitions_unscaled_coroutine(){
        if(CameraEffects.instance != null){
            CameraEffects.instance.completed_fade_to_black += load_scene; // has to be linked beforehand to ensure the IEnumerator instance of the action isnt null.
            CameraEffects.instance?.fade_to_black_unscaled(1);
        }
        else
            load_scene();
        yield break;
    } 

    static void set_game_data(){
        GameManager.data.scene_to_load = scene_to_load;
    }
}
