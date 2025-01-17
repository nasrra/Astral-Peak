using System;
using System.Linq;
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
    static GameState state = GameState.GAMEPLAY;
    public static Action<GameState> 
        entered_game_state, 
        exited_game_state;
    public static bool[] boss_states = Enumerable.Repeat(false, 3).ToArray();
    //public static bool[] boss_states = Enumerable.Repeat(true, 3).ToArray();


    public static void initialize(){
        state_changed(GameState.MENU);
        #if UNITY_EDITOR
        state_changed(GameState.GAMEPLAY);
        #endif
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
}
