using UnityEngine;
using UnityEngine.InputSystem;


// base.uninitialize is used as an animation event.

public class ButtonPromptHUD : ButtonPrompt{
    [Header("ButtonPromptHUD")]
    [SerializeField] Animator animator;
    public override void initialize(){
        base.initialize();
        Application.quitting += unitialize;
    }
    
    // used in the animator.
    public void link_action(){
        action.performed += turn_off;
    }

    public void turn_off(InputAction.CallbackContext context){
        animator.Play("turn_off");
        action.performed -= turn_off;
        Application.quitting -= unitialize;
    }
}
