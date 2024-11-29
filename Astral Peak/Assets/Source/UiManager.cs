using System;
using System.Collections;
using DocumentFormat.OpenXml.Wordprocessing;
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
        screen_transitions;
    [SerializeField] DialogueHandler dialogue; 
    AudioSource source;
    
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

    public void play_enemy_vanquished() => StartCoroutine(enemy_vanquished_state());
    IEnumerator enemy_vanquished_state(){
        AudioClipHandler.play(
            SoundID.WOODEN_PING,
            audio_player:       this, 
            source:             out source, 
            randomise_pitch:    false, 
            spatial_blend:      false,
            loop:               false);
        enemy_vanquished.SetActive(true);
        yield return new WaitForSeconds(4);
        yield break;
    }

    public void play_death_screen() => StartCoroutine(death_screen_state());
    IEnumerator death_screen_state(){
        AudioClipHandler.play(
            SoundID.WOODEN_PING,
            audio_player:       this, 
            source:             out source, 
            randomise_pitch:    false, 
            spatial_blend:      false,
            loop:               false);
        death_screen.SetActive(true);
        yield return new WaitForSeconds(4);
        UiManager.instance.fade_to_black();
        yield return new WaitForSeconds(4);  
        death_screen_ended?.Invoke();
        yield break;
    }

    public void start_dialogue() => dialogue.start_dialogue();
    public void next_dialogue_line() => dialogue.next_line();
    public DialogueHandler get_dialogue_handler() => dialogue;
    public void fade_to_black() => screen_transitions.Play("fade_to_black");
    public void fade_from_black() => screen_transitions.Play("fade_from_black");

    void link() => InputManager.exit_performed += gameplay_ui;
    void unlink() => InputManager.exit_performed -= gameplay_ui;
}

public enum UiState{
    HUD,
    SETTINGS,
    ENEMY_VANQUISHED,
    DEATH,
}