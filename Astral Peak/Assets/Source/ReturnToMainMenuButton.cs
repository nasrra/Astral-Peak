using UnityEngine;

public class ReturnToMainMenuButton : MonoBehaviour{
    public void invoke() => CustomSceneManager.load_scene("MainMenu");
}
