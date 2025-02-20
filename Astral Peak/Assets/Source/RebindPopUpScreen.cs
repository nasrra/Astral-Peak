using TMPro;
using UnityEngine;

public class RebindPopUpScreen : MonoBehaviour{
    [SerializeField] GameObject graphics;
    [SerializeField] TextMeshProUGUI press_key_text;
    [SerializeField] TextMeshProUGUI invalid_key_text;
    void Awake(){
        disable_graphics();
        InputManager.rebind_started     += enable_graphics;
        InputManager.rebind_failed      += invalid_key_pressed;
        InputManager.rebind_completed   += rebind_completed;
    }

    void enable_graphics(){
        graphics.SetActive(true);
        press_key_text.enabled = true;
    }

    void invalid_key_pressed(){
        invalid_key_text.enabled = true;
        AudioManager.play_non_diegetic_one_shot("ui_button_rejected");
    }

    void rebind_completed(){
        AudioManager.play_non_diegetic_one_shot("ui_button_accepted");
        disable_graphics();
    }

    void disable_graphics(){
        graphics.SetActive(false);
        press_key_text.enabled = false;
        invalid_key_text.enabled = false;
    }

    void OnDestroy(){
        InputManager.rebind_started     -= enable_graphics;
        InputManager.rebind_failed      -= invalid_key_pressed;
        InputManager.rebind_completed   -= rebind_completed;
    }
}
