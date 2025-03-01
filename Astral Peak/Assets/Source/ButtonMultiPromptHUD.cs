using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ButtonMultiPromptHUD : MonoBehaviour{
    [Header("Button Combination Prompt")]
    [SerializeField] Animator animator;
    [SerializeField] protected List<ButtonPrompt> buttons = new List<ButtonPrompt>();

    protected void initialize(){
        foreach(ButtonPrompt button in buttons){
            button.initialize();
        }
        Application.quitting += uninitialize;
    }

    public void link_action(){
        foreach(ButtonPrompt button in buttons){
            button.action.performed += handle_pressed_action;
        }
    }

    protected void handle_pressed_action(InputAction.CallbackContext ctx){
        animator.Play("turn_off");
        foreach(ButtonPrompt button in buttons){
            button.action.performed -= handle_pressed_action;
        }
    }

    protected void uninitialize(){
        foreach(ButtonPrompt button in buttons){
            button.action.performed -= handle_pressed_action;
            button.unitialize();
        }
        Application.quitting -= uninitialize;
    }
}
