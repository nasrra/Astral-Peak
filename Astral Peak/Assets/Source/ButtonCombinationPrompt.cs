using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ButtonCombinationPromptHUD : ButtonPromptHUD{
    protected override void turn_off_wrapper(InputAction.CallbackContext context){
        if(pressed.Count >= buttons.Count)
            turn_off();
    }

    public override void now_turned_on(){
        turned_on = true;
        // check if the button has already been pressed.
        get_pressed_keys();
        if(pressed.Count >= buttons.Count)
            turn_off();
    }
}
