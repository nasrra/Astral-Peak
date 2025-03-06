using UnityEngine;

public class MainMenu : MonoBehaviour{
    void Awake(){
        GameManager.state_changed(GameState.MENU);
    }
    void Start(){
        AudioManager.play_music("music_main_menu");
        CameraEffects.instance.astral_plane_state();
    }
}
