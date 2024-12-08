using System;
using System.Collections;
using DocumentFormat.OpenXml.Presentation;
using UnityEngine;

public class UiManager : MonoBehaviour{
    public event Action
        death_screen_ended;
    public static UiManager instance;
    [SerializeField] UiState state;
    [SerializeField] GameObject 
        death_screen,
        settings_menu,
        hud,
        enemy_vanquished;
    [SerializeField] Animator
        black_bars; 
    [SerializeField] DialogueHandler dialogue; 
    Coroutine hud_fade;

    void OnEnable(){
        instance = this;
        GameManager.link_Ui();
        link();
    }

    void OnDisable(){
        GameManager.unlink_Ui();
        unlink();   
    }

    void gameplay_ui(){
        death_screen.SetActive(false);
        settings_menu.SetActive(false);
        hud.SetActive(false);
        switch(state){
            case UiState.HUD:
                settings_menu.SetActive(true);
                state = UiState.SETTINGS;
                break;
            case UiState.SETTINGS:
                hud.SetActive(true);
                state = UiState.HUD;
                break;
            default: 
                break;
        }
    }

    public void enable_death_screen(){
        settings_menu.SetActive(false);
        hud.SetActive(false);
        play_death_screen();
    }

    IEnumerator lerp_colours(Color start, Color end, float time){
        float elapsedTime = 0f;
        while (elapsedTime < time){
            elapsedTime += Time.deltaTime;
            start = Color.Lerp(start, end, elapsedTime/time/100); // have to divide by 100 for some reason, dunno why lol.
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        start = end;
        yield break;
    }


    public void play_enemy_vanquished() => StartCoroutine(enemy_vanquished_state());
    IEnumerator enemy_vanquished_state(){
        AudioClipHandler.play(
            SoundID.WOODEN_PING,
            audio_player:       this, 
            AudioSourceSettings.NON_DIEGETIC);  
        enemy_vanquished.SetActive(true);
        yield return new WaitForSeconds(4);
        yield break;
    }

    public void play_death_screen() => StartCoroutine(death_screen_state());
    IEnumerator death_screen_state(){
        AudioClipHandler.play(
            SoundID.WOODEN_PING,
            audio_player:       this, 
            AudioSourceSettings.NON_DIEGETIC);  

        death_screen.SetActive(true);
        yield return new WaitForSeconds(4);
        CameraEffects.instance.fade_to_black();
        yield return new WaitForSeconds(4);  
        death_screen_ended?.Invoke();
        yield break;
    }

    public void cutscene_mode(bool x) => black_bars.Play(x==true?"fade_in":"fade_out");

    public void start_dialogue() => dialogue.start_dialogue();
    public void next_dialogue_line() => dialogue.next_line();
    public DialogueHandler get_dialogue_handler() => dialogue;

    void link() => InputManager.exit_performed += gameplay_ui;
    void unlink() => InputManager.exit_performed -= gameplay_ui;
}

public enum UiState{
    HUD,
    SETTINGS,
    ENEMY_VANQUISHED,
    DEATH,
}