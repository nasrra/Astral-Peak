using System.Collections.Generic;
using UnityEngine;

public class ResetKeybindsButton : MonoBehaviour{
    [SerializeField] string keybinds;
    [SerializeField] List<ButtonPromptUi> ui_buttons = new List<ButtonPromptUi>();
    public void invoke(){
        InputManager.reset_keybinds(keybinds);
        foreach(ButtonPromptUi button in ui_buttons){
            button.unitialize();
            button.initialize();
        }
    }
}
