using UnityEngine;

public class ButtonPromptTrigger : MonoBehaviour{
    [SerializeField] string button;
    [SerializeField] bool flag = false;
    void OnTriggerEnter2D(){
        if(flag == false){
            UiManager.instance.enable_button_prompt(button);
            flag = true;
        }
    }
}
