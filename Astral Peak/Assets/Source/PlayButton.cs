using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour{
    public void load_game() => CustomSceneManager.load_scene_with_transitions("Shrine");
}
