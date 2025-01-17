using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour{
    public void load_game(){
        GameManager.load_game_data();
        CustomSceneManager.load_scene_with_transitions("Shrine");
    }
}
