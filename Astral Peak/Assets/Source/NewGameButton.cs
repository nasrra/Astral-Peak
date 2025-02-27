using Entropek;
using UnityEngine;

public class NewGameButton : MonoBehaviour{
    public void invoke(){
        GameManager.new_game();
        AudioManager.stop_music();
        CustomSceneManager.load_scene_with_transitions("Introduction");
    }
}
