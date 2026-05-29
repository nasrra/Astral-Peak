using UnityEngine;

public class ReturnToMainMenuButton : MenuButton{
    public void invoke(){
        disable_button();
        AudioManager.stop_music();
        AudioManager.stop_ambience();
        AudioManager.stop_additive_ambience();
        AudioManager.exit_low_pass_filter();
        GameManager.invoke_set_game_data();
        GameManager.save_game_data();
        CustomSceneManager.load_scene_with_transitions_unscaled("MainMenu");
    }
}
