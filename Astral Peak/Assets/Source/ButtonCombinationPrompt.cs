using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ButtonCombinationPromptUI : MonoBehaviour{
    [Header("Button Combination Prompt")]
    [SerializeField] Animator animator;
    [SerializeField] protected List<ButtonPrompt> buttons = new List<ButtonPrompt>();
    protected Dictionary<InputAction, bool> pressed = new Dictionary<InputAction, bool>();

    protected void initialize(){
        foreach(ButtonPrompt button in buttons){
            button.initialize();
            pressed.Add(button.action, false);
        }
        Application.quitting += uninitialize;
    }

    public void link_action(){
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

    protected void uninitialize(){
        foreach(ButtonPrompt button in buttons){
            button.unitialize();
        }
        Application.quitting -= uninitialize;
    }
}
