using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuButton : MenuInteractable{
    [SerializeField] Animator animator;
    public override void pointer_enter(){
        animator.Play("MenuButtonPointerEnter");
        base.pointer_enter();
    }
    public void pointer_exit(){
        animator.Play("MenuButtonPointerExit");
    }
}
