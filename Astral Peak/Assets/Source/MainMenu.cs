using UnityEngine;

public class MainMenu : MonoBehaviour{
    void Awake(){
        GameManager.state_changed(GameState.MENU);
    }
}
