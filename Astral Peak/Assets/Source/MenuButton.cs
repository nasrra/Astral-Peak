using UnityEngine;
using UnityEngine.EventSystems;

public class MenuButton : MonoBehaviour{
    [SerializeField] Animator animator;
    public void pointer_enter(){
        AudioManager.play_non_diegetic_one_shot("ui_button_hovered");
        animator.Play("MenuButtonPointerEnter");
    }
    public void pointer_exit(){
        animator.Play("MenuButtonPointerExit");
    }
    public void on_click(){
        AudioManager.play_non_diegetic_one_shot("ui_button_accepted");
    }
}
