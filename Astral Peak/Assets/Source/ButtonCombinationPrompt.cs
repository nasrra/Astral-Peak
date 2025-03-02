using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ButtonCombinationPromptHUD : ButtonPromptHUD{
    [Header("Button Combination Prompt")]
    protected Dictionary<InputAction, bool> pressed = new Dictionary<InputAction, bool>();

    protected override void initialize(){
        foreach(ButtonPrompt button in buttons){
            button.initialize();
            pressed.Add(button.action, false);
        }
        Application.quitting += uninitialize;
    }

    protected override void link_action(){
        foreach(ButtonPrompt button in buttons){
            button.action.performed += handle_pressed_action;
            button.action.canceled  += handle_canceled_action;        
        }
    }

    protected void handle_pressed_action(InputAction.CallbackContext ctx){
        pressed[ctx.action] = true;
        if(pressed.ContainsValue(false) == false){
            animator.Play("turn_off");
            foreach(ButtonPrompt button in buttons){
                button.action.performed -= handle_pressed_action;
                button.action.canceled  -= handle_canceled_action;
            }
            pressed.Clear();
        }       
    }

    protected void handle_canceled_action(InputAction.CallbackContext ctx){
        pressed[ctx.action] = false;
    }

    protected override void uninitialize(){
        foreach(ButtonPrompt button in buttons){
            if(button.action != null){
                button.action.performed -= handle_pressed_action;
                button.action.canceled  -= handle_canceled_action;
            }
        }
        foreach(ButtonPrompt button in buttons){
            button.unitialize();
        }
        Application.quitting -= uninitialize;
    }
}
