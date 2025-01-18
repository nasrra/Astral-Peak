using UnityEngine;
using UnityEngine.InputSystem;


// base.uninitialize is used as an animation event.

public class ButtonPromptHUD : ButtonPrompt{
    [Header("ButtonPromptHUD")]
    [SerializeField] Animator animator;
    public override void initialize(){
        base.initialize();
        action.performed += turn_off;
        Application.quitting += unitialize;
    }

    public void turn_off(InputAction.CallbackContext context){
        animator.Play("turn_off");
        action.performed -= turn_off;
        Application.quitting -= unitialize;
    }
}
