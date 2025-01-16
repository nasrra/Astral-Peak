using System;
using System.Collections;
using DocumentFormat.OpenXml.Presentation;
using UnityEngine;
using Sounds;
using AYellowpaper.SerializedCollections;

public class UiManager : MonoBehaviour{
    public event Action
        death_screen_ended;
    public static UiManager instance;
    [SerializeField] SerializedDictionary<string, Animator> button_prompts = new SerializedDictionary<string, Animator>();
    [SerializeField] UiState state;
    [SerializeField] GameObject 
        death_screen,
        settings_menu,
        hud,
        enemy_vanquished;
    [SerializeField] Animator
        black_bars; 
    [SerializeField] DialogueHandler dialogue; 
    [SerializeField] PlayerHealthBar health_bar;

    void Awake(){
        link_statics();
        instance = this;
        GameManager.link_Ui();
    }

    void Start(){
        link_instances();
    }

    void OnDestroy(){
        GameManager.unlink_Ui();
        unlink_statics();
        unlink_instances();   
    }

    void gameplay_ui(){
        if(GameManager.get_state() == GameState.CUTSCENE)
            return;
        death_screen.SetActive(false);
        settings_menu.SetActive(false);
        hud.SetActive(false);
        switch(state){
            case UiState.HUD:
                settings_menu.SetActive(true);
                state = UiState.SETTINGS;
                GameManager.state_changed(GameState.MENU);
                break;
            case UiState.SETTINGS:
                hud.SetActive(true);
                state = UiState.HUD;
                GameManager.state_changed(GameState.GAMEPLAY);
                break;
            default: 
                break;
        }
    }

    public void enable_button_prompt(string button) => button_prompts[button].Play("turn_on");

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
        CameraEffects.instance.fade_to_black(1);
        yield return new WaitForSeconds(4);  
        death_screen_ended?.Invoke();
        yield break;
    }

    void enable_black_bars() => black_bars.Play("fade_in");
    void disable_black_bars() => black_bars.Play("fade_out");

    public void start_dialogue() => dialogue.start_dialogue();
    public void next_dialogue_line() => dialogue.next_line();
    public DialogueHandler get_dialogue_handler() => dialogue;

    void entered_game_state(GameState state){
        if(state == GameState.CUTSCENE){
            health_bar.fade_out();
            enable_black_bars();
        }
    }

    void exited_game_state(GameState state){
        if(state == GameState.CUTSCENE){
            health_bar.fade_in();
            disable_black_bars();
        }
    }

    void link_statics(){
        GameManager.entered_game_state                  += entered_game_state;
        GameManager.exited_game_state                   += exited_game_state;
        InputManager.exit_performed                     += gameplay_ui;
    }
    void link_instances(){
        CameraEffects.instance.started_fade_to_black    += health_bar.fade_out;
        CameraEffects.instance.started_fade_from_black  += health_bar.fade_in;
    }
    void unlink_statics(){
        GameManager.entered_game_state                  -= entered_game_state;
        GameManager.exited_game_state                   -= exited_game_state;
        InputManager.exit_performed                     -= gameplay_ui;
    }
    void unlink_instances(){
        CameraEffects.instance.started_fade_to_black    -= health_bar.fade_out;
        CameraEffects.instance.started_fade_from_black  -= health_bar.fade_in;
    }
}

public enum UiState{
    HUD,
    SETTINGS,
    ENEMY_VANQUISHED,
    DEATH,
}