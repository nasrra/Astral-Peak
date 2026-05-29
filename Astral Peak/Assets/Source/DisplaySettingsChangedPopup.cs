using System;
using System.Collections;
using Entropek;
using TMPro;
using UnityEngine;

public class DisplaySettingsChangedPopup : MonoBehaviour{
    [SerializeField] GameObject graphics;
    [SerializeField] TextMeshProUGUI timer_text;
    Coroutine timer;

    void Awake(){
        DisplaySettingsManager.change_buffer_started    += enable_pop_up;
        DisplaySettingsManager.change_buffer_cancelled  += disable_pop_up;
        DisplaySettingsManager.change_buffer_accepted   += disable_pop_up;
    }

    void enable_pop_up(){
        graphics.SetActive(true);
        timer = StartCoroutine(timer_text_coroutine(DisplaySettingsManager.change_buffer_time));
    }

    void disable_pop_up(){
        StopCoroutine(timer);
        graphics.SetActive(false);
    }

    public void confirm_button_pressed(){
        DisplaySettingsManager.accept_display_settings_change();
        disable_pop_up();
    }

    public void reject_button_pressed(){
        DisplaySettingsManager.cancel_display_settings_change();
        disable_pop_up();
    }

    IEnumerator timer_text_coroutine(float _time){
        float time = _time;
        while(time >= 0){
            time -= 0.1f;
            timer_text.text = $"({time:F1})"; // to one decimal place
            timer_text.ForceMeshUpdate();
            yield return new WaitForSecondsRealtime(0.1f);
        }
        time = 0;
        timer_text.text = $"({time})";
        timer_text.ForceMeshUpdate();
        yield break; 
    }

    void OnDestroy(){
        DisplaySettingsManager.change_buffer_started    -= enable_pop_up;
        DisplaySettingsManager.change_buffer_cancelled  -= disable_pop_up;
        DisplaySettingsManager.change_buffer_accepted   -= disable_pop_up;    
    }
}
