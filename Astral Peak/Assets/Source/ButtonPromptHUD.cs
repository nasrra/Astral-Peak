using System.Collections.Generic;
using Entropek;
using UnityEngine;
using UnityEngine.InputSystem;


// base.uninitialize is used as an animation event.

public class ButtonPromptHUD : MonoBehaviour{
    [Header("ButtonPromptHUD")]
    [SerializeField] protected List<ButtonPrompt> buttons = new List<ButtonPrompt>();
    [SerializeField] protected Animator animator;
    [SerializeField] protected List<bool> pressed = new List<bool>();
    [SerializeField] protected bool turned_on = false;

    void OnDisable(){
        unlink();
    }

    protected virtual void initialize(){
        foreach(ButtonPrompt button in buttons)
            button.initialize();
        // link action
        foreach(ButtonPrompt button in buttons){
            button.action.performed += button_pressed;
            button.action.performed += turn_off_wrapper;
            button.action.canceled  += button_cancelled;
        }
        Application.quitting += uninitialize;
    }
    
    protected virtual void uninitialize(){
        foreach(ButtonPrompt button in buttons)
            button.unitialize();
    }

    void button_cancelled(InputAction.CallbackContext context){
        if(pressed.Count > 0)
            pressed.RemoveAt(0);
    }

    void button_pressed(InputAction.CallbackContext context){
        pressed.Add(true);
    }

    public virtual void now_turned_on(){
        turned_on = true;
        // check if the button has already been pressed.
        get_pressed_keys();
        if(pressed.Count > 0)
            turn_off();
    }

    protected void get_pressed_keys(){
        foreach(ButtonPrompt button in buttons)
            if(button.action.IsPressed() == true)
                pressed.Add(true);
    }

    public void turn_on(){
        animator.Play("turn_on");
    }

    protected virtual void turn_off_wrapper(InputAction.CallbackContext context){
        turn_off();
    }

    public void turn_off(){
        if(turned_on == false)
            return;
        animator.Play("turn_off");
        unlink();
    }

    public void off(){
        animator.Play("off");
        unlink();
    }

    protected virtual void unlink(){
        // Debug.Log(gameObject.name);
        foreach(ButtonPrompt button in buttons){
            if(button.action != null){
                button.action.performed -= turn_off_wrapper;
                button.action.performed -= button_pressed;
                button.action.canceled  -= button_cancelled;
            }
        }
        pressed.Clear();
        Application.quitting -= uninitialize;
    }
}
