using UnityEngine;

public class ButtonPromptTrigger : MonoBehaviour{
    [SerializeField] string button;
    [SerializeField] Collider2D col;
    void OnTriggerEnter2D(){
        UiManager.instance.enable_button_prompt(button);
        col.enabled = false;
    }
}
