using Entropek;
using UnityEngine;

public class ContinueButton : MenuButton{
    [SerializeField] GameObject
        active_button,
        inactive_button;
    void OnEnable(){
        active_button.SetActive(false);
        inactive_button.SetActive(false);
        if(FileManager.file_exists())
            active_button.SetActive(true);
        else
            inactive_button.SetActive(true);
    }
    public void load_game(){
        disable_button();
        AudioManager.stop_music();
        CustomSceneManager.load_scene_with_transitions(GameManager.load_game_data().scene_to_load);
    }
}
