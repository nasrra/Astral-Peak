using System;
using System.Linq;
using Entropek;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState{
    PAUSE,
    MENU,
    GAMEPLAY,
    DEATH,
    CUTSCENE,
}

public static class GameManager{
    public static Action set_game_data;
    static GameState state = GameState.GAMEPLAY;
    static GameData data = new GameData();
    static bool data_loaded = false;

    public static Action<GameState> 
        entered_game_state, 
        exited_game_state;
    public static bool[] boss_states = Enumerable.Repeat(false, 3).ToArray();

    public static void initialize(){
        state_changed(GameState.MENU);
        #if UNITY_EDITOR
        state_changed(GameState.GAMEPLAY);
        #endif
    }
    public static void uninitialize(){
        CustomSceneManager.loaded_scene -= link_loaded_scene_to_load_data;
        CustomSceneManager.loading_scene += invoke_set_game_data;
    }

    public static GameState get_state() => state;

    static public void death_state(){
        UiManager.instance.enable_death_screen();
    }

    static public void reload_scene() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    static public void state_changed(GameState _state){
        GameState previous = state;
        exited_game_state?.Invoke(previous);
        entered_game_state?.Invoke(_state);
        state = _state;
    }

    public static void link_player() => Player.instance.death_completed += death_state;
    public static void unlink_player() => Player.instance.death_completed -= death_state;
    public static void link_Ui() => UiManager.instance.death_screen_ended += reload_scene;
    public static void unlink_Ui() => UiManager.instance.death_screen_ended -= reload_scene;
    public static void set_boss_state(int boss, bool completed) => boss_states[boss]=completed;
    public static bool get_boss_state(int boss) => boss_states[boss];
    public static void pause_game(bool pause) => Time.timeScale = pause ? 0 : 1;
    public static GameData get_game_data() => data;
    public static void save_game_data() => FileManager.save_data(data);
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
    public static void invoke_set_game_data(){
        if(state != GameState.CUTSCENE){
            data.boss_states = boss_states;
            set_game_data?.Invoke();
        }
    }
}
