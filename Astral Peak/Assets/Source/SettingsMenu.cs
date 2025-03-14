using UnityEngine;

public class SettingsMenu : MonoBehaviour{
    void OnEnable(){
        
    }
    void OnDisable(){
        DisplaySettingsManager.save_player_prefs();
    }
}
