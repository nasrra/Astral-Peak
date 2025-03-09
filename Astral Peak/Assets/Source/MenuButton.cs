using UnityEngine;
using UnityEngine.UI;

public class MenuButton : MenuInteractable{
    [SerializeField] protected Button button;
    [SerializeField] protected Animator animator;
    public override void pointer_enter(){
        animator.Play("MenuButtonPointerEnter");
        base.pointer_enter();
    }
    public void pointer_exit(){
        animator.Play("MenuButtonPointerExit");
    }
    public void disable_button(){
        pointer_exit();
        button.enabled = false; // disable button to stop multiple presses.
    }
}
