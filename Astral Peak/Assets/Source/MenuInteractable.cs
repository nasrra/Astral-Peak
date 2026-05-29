using UnityEngine;

public class MenuInteractable : MonoBehaviour{
    public virtual void pointer_enter(){
        AudioManager.play_non_diegetic_one_shot("ui_button_hovered");
    }
    public virtual void pointer_click(){
        AudioManager.play_non_diegetic_one_shot("ui_button_accepted");
    }
}
