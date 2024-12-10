using System;
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
    public static int world_state = 0;

    public static void initialize(){
        world_state = 0;
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

    static public void link_player() => Player.instance.death += death_state;
    static public void unlink_player() => Player.instance.death -= death_state;
    static public void link_Ui() => UiManager.instance.death_screen_ended += reload_scene;
    static public void unlink_Ui() => UiManager.instance.death_screen_ended -= reload_scene;
    public static void increment_world_state() => ++world_state;
}
