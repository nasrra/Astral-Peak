using UnityEngine;

public class ReturnToMainMenuButton : MonoBehaviour{
    public void invoke(){
        GameManager.invoke_set_game_data();
        GameManager.save_game_data();
        CustomSceneManager.load_scene_with_transitions_unscaled("MainMenu");
    }
}
