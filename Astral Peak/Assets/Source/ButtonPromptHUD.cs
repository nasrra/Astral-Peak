using UnityEngine;
using UnityEngine.InputSystem;

public class ButtonPromptHUD : ButtonPrompt{
    [Header("ButtonPromptHUD")]
    [SerializeField] Animator animator;
    public override void initialize(){
        base.initialize();
        action.performed += uninitialize_wrapper;
    }

    private void uninitialize_wrapper(InputAction.CallbackContext context) => unitialize();
    public override void unitialize(){
        base.unitialize();
        animator.Play("turn_off");
        action.performed -= uninitialize_wrapper;
    }
}
