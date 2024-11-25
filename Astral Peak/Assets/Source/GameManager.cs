using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

enum GameState{
    MENU,
    GAMEPLAY,
    DEATH,
    CUTSCENE,
}

public static class GameManager{
    static GameState state;

    static public void gameplay_state(){

    }

    static public void death_state(){
        UiManager.instance.enable_death_screen();
    }

    static public void reload_scene() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    static public void cutscene_state(){
    }

    static public void link_player() => Player.instance.death += death_state;
    static public void unlink_player() => Player.instance.death -= death_state;
    static public void link_Ui() => UiManager.instance.death_screen_ended += reload_scene;
    static public void unlink_Ui() => UiManager.instance.death_screen_ended -= reload_scene;
}
