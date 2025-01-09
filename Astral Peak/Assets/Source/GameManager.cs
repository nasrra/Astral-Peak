using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

public enum GameState{
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
    public static List<bool> boss_states = Enumerable.Repeat(false, 3).ToList();

    public static void initialize(){
        state = GameState.GAMEPLAY;
    }

    public static GameState get_state() => state;

    static public void death_state(){
        UiManager.instance.enable_death_screen();
    }

    static public void reload_scene() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    static public void state_changed(GameState _state){
        entered_game_state?.Invoke(_state);
        GameState previous = state;
        state = _state;
        exited_game_state?.Invoke(previous);
    }

    public static void link_player() => Player.instance.death_completed += death_state;
    public static void unlink_player() => Player.instance.death_completed -= death_state;
    public static void link_Ui() => UiManager.instance.death_screen_ended += reload_scene;
    public static void unlink_Ui() => UiManager.instance.death_screen_ended -= reload_scene;
    public static void set_boss_state(int boss, bool completed) => boss_states[boss]=completed;
    public static bool get_boss_state(int boss) => boss_states[boss];
}
