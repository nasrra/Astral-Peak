using Entropek;
using UnityEngine;

public class NewGameButton : MenuButton{
    public void invoke(){
        disable_button();
        GameManager.new_game();
        AudioManager.stop_music();
        CustomSceneManager.load_scene_with_transitions("Introduction");
    }
}
