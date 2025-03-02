using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


// base.uninitialize is used as an animation event.

public class ButtonPromptHUD : MonoBehaviour{
    [Header("ButtonPromptHUD")]
    [SerializeField] protected List<ButtonPrompt> buttons = new List<ButtonPrompt>();
    [SerializeField] protected Animator animator;

    void OnDisable(){
        unlink();
    }

    protected virtual void initialize(){
        foreach(ButtonPrompt button in buttons)
            button.initialize();
        Application.quitting += uninitialize;
    }
    
    protected virtual void uninitialize(){
        foreach(ButtonPrompt button in buttons)
            button.unitialize();
    }

    // used in the animator.
    protected virtual void link_action(){
        foreach(ButtonPrompt button in buttons)
            button.action.performed += turn_off;
    }

    public void turn_on(){
        animator.Play("turn_on");
    }

    public void turn_off(InputAction.CallbackContext context){
        animator.Play("turn_off");
        unlink();
    }

    public void off(){
        animator.Play("off");
        unlink();
    }

    protected virtual void unlink(){
        foreach(ButtonPrompt button in buttons){
            if(button.action != null)
                button.action.performed -= turn_off;
        }
        Application.quitting -= uninitialize;
    }
}
