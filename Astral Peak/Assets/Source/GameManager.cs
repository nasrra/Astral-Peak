using System;
using System.Linq;
using Entropek;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState{
    MENU,
    GAMEPLAY,
    DEATH,
    CUTSCENE,
}

public static class GameManager{
    public static event Action set_game_data;
    static GameState state = GameState.GAMEPLAY;
    public static GameData data {get; private set;}
    static bool data_loaded = false;
    static float time_scale = 1;

    public static Action<GameState> 
        entered_game_state, 
        exited_game_state;

    public static void initialize(){
        data = new GameData();
        swap_state(GameState.MENU);
        #if UNITY_EDITOR
        swap_state(GameState.GAMEPLAY);
        #endif
    }
    public static void uninitialize(){
        CustomSceneManager.loaded_scene    -= link_loaded_scene_to_load_data;
        CustomSceneManager.preparing_scene_load += invoke_set_game_data;
    }

    public static GameState get_state() => state;

    static public void death_state(){
        UiManager.instance.enable_death_screen();
    }

    static public void reload_scene(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        InputManager.enable_user_input(); // here to re-enable player input that is turned off when player death starts.
    }

    static public void swap_state(GameState _state){
        GameState previous = state;
        if(_state == GameState.MENU)
            Cursor.visible = true;
        else
            Cursor.visible = false;
        exited_game_state?.Invoke(previous);
        entered_game_state?.Invoke(_state);
        state = _state;
    }

    public static void link_player() => Player.instance.death_completed += death_state;
    public static void unlink_player() => Player.instance.death_completed -= death_state;
    public static void link_Ui() => UiManager.instance.death_screen_ended += reload_scene;
    public static void unlink_Ui() => UiManager.instance.death_screen_ended -= reload_scene;
    public static void set_boss_state(int boss, bool completed) => data.boss_states[boss]=completed;
    public static bool get_boss_state(int boss) => data.boss_states[boss];
    public static void pause_game(bool pause) => Time.timeScale = pause ? 0 : time_scale;
    public static void set_time_scale(float _time_scale){
        Time.timeScale = _time_scale;
        time_scale = _time_scale;
    }
    public static void save_game_data(){
        if(state != GameState.CUTSCENE)
            FileManager.save_data(data);
    }
    public static GameData load_game_data(){
        data_loaded = true;
        CustomSceneManager.loaded_scene += link_loaded_scene_to_load_data;
        data = FileManager.load_data<GameData>();
        return data;
    }
    private static void link_loaded_scene_to_load_data(){
        data_loaded = false;
        CustomSceneManager.loaded_scene -= link_loaded_scene_to_load_data;
    }
    public static bool is_data_loaded() => data_loaded;
    public static void new_game(){
        FileManager.delete_data(); // delete the previous save file.
        data = new GameData();        
    }
    public static void invoke_set_game_data(){
        if(state != GameState.CUTSCENE){
            set_game_data?.Invoke();
        }
    }
    public static void boss_defeated(int boss) => data.boss_states[boss] = true;
}