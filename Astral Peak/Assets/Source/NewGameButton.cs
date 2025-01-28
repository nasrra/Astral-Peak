using Entropek;
using UnityEngine;

public class NewGameButton : MonoBehaviour{
    public void invoke(){
        GameManager.new_game();
        CustomSceneManager.load_scene_with_transitions("Introduction");
    }
}
