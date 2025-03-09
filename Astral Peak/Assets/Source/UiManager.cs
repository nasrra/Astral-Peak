using System;
using System.Collections;
using UnityEngine;
using AYellowpaper.SerializedCollections;
using Entropek;

public class UiManager : MonoBehaviour{
    public event Action
        death_screen_ended;
    public static UiManager instance;
    [SerializeField] SerializedDictionary<string, ButtonPromptHUD> button_prompts = new SerializedDictionary<string, ButtonPromptHUD>();
    [SerializeField] GameObject 
        death_screen,
        pause_menu,
        hud,
        settings_menu,
        enemy_vanquished;
    [SerializeField] Animator
        black_bars; 
    [SerializeField] DialogueHandler dialogue; 
    [SerializeField] PlayerHealthBar health_bar;
    [SerializeField] AudioPlayer audio_player;
    [HideInInspector] public bool lock_gameplay_ui_toggle = false;

    void Awake(){
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.playModeStateChanged += handle_play_mode_state_changed;
        #endif
        link_statics();
        instance = this;
        GameManager.link_Ui();
        check_game_state();
    }

    void Start(){
        link_instances();
    }

    void OnDestroy(){
        GameManager.unlink_Ui();
        unlink_statics();
        unlink_instances();   
        GameManager.pause_game(false);
    }

    public void toggle_gameplay_ui(){
        if(lock_gameplay_ui_toggle == true || GameManager.get_state() == GameState.CUTSCENE || GameManager.get_state() == GameState.DEATH)
            return;
        if(hud.activeSelf == true)
            enable_pause_menu();
        else if(pause_menu.activeSelf == true)
            disable_pause_menu();
        else if(settings_menu.activeSelf == true){
            settings_menu.SetActive(false);
            pause_menu.SetActive(true);
        }
    }

    private void enable_pause_menu(){
        hud.SetActive(false);
        pause_menu.SetActive(true);
        GameManager.swap_state(GameState.MENU);
        GameManager.pause_game(true);        
    }

    private void disable_pause_menu(){
        pause_menu.SetActive(false);
        hud.SetActive(true);
        GameManager.swap_state(GameState.GAMEPLAY);
        GameManager.pause_game(false);       
    }

    public void check_game_state(){
        if(GameManager.get_state() == GameState.CUTSCENE)
            cutscene_state_on();
    }

    public void enable_button_prompt(string button){
        GameData data = GameManager.data; 
        if(data.tutorials_completed[button] == false){
            button_prompts[button].turn_on();
            data.tutorials_completed[button] = true;
        }
    }

    public void enable_death_screen(){
        pause_menu.SetActive(false);
        hud.SetActive(false);
        play_death_screen();
    }

    public void play_enemy_vanquished() => StartCoroutine(enemy_vanquished_state());
    IEnumerator enemy_vanquished_state(){
        audio_player.play_non_diegetic_one_shot("ui_death_screen");
        enemy_vanquished.SetActive(true);
        yield return new WaitForSeconds(4);
        yield break;
    }

    public void play_death_screen() => StartCoroutine(death_screen_state());
    IEnumerator death_screen_state(){
        audio_player.play_non_diegetic_one_shot("ui_death_screen");
        death_screen.SetActive(true);
        yield return new WaitForSeconds(4);
        CameraEffects.instance.fade_to_black(1);
        yield return new WaitForSeconds(4);  
        death_screen_ended?.Invoke();
        yield break;
    }

    void fade_in_black_bars() => black_bars.Play("fade_in");
    void fade_out_black_bars() => black_bars.Play("fade_out");
    void black_bars_on() => black_bars.Play("on");
    void black_bars_off() => black_bars.Play("off");

    void fade_in_cutscene_state(){
        fade_out();
        fade_in_black_bars();
    }

    void fade_out_cutscene_state(){
        health_bar.fade_in();
        fade_out_black_bars();
    }

    void cutscene_state_on(){
        black_bars_on();
        health_bar.off();        
    }

    public void start_dialogue() => dialogue.start_dialogue();
    public void next_dialogue_line() => dialogue.next_line();
    public DialogueHandler get_dialogue_handler() => dialogue;

    void entered_game_state(GameState state){
        if(state == GameState.CUTSCENE)
            fade_in_cutscene_state();
    }

    void exited_game_state(GameState state){
        if(state == GameState.CUTSCENE)
            fade_out_cutscene_state();
    }

    void fade_out(){
        if(GameManager.get_state() != GameState.CUTSCENE){
            health_bar.fade_out();
            // foreach(ButtonPromptHUD button in button_prompts.Values)
            //     if(button.isActiveAndEnabled == true)
            //         button.off();
        }
    }
    void fade_in(){
        if(GameManager.get_state() != GameState.CUTSCENE){
            health_bar.fade_in();
        }
    }

    #if UNITY_EDITOR
    private void handle_play_mode_state_changed(UnityEditor.PlayModeStateChange state){
        if (state == UnityEditor.PlayModeStateChange.ExitingPlayMode){
            UnityEditor.EditorApplication.playModeStateChanged -= handle_play_mode_state_changed;
            unlink_instances();
        }
    }
    #endif

    void link_statics(){
        GameManager.entered_game_state                  += entered_game_state;
        GameManager.exited_game_state                   += exited_game_state;
        link_player();
        link_pause_menu_toggle();
    }
    void link_instances(){
        CameraEffects.instance.started_fade_to_black    += fade_out;
        CameraEffects.instance.started_fade_from_black  += fade_in;
    }
    void unlink_statics(){
        GameManager.entered_game_state                  -= entered_game_state;
        GameManager.exited_game_state                   -= exited_game_state;
        unlink_player();
        unlink_pause_menu_toggle();
    }
    void unlink_instances(){
        CameraEffects.instance.started_fade_to_black    -= fade_out;
        CameraEffects.instance.started_fade_from_black  -= fade_in;
    }
    void link_player(){
        Player.instance.entered_door                    += unlink_pause_menu_toggle;
        Player.instance.entered_door                    += disable_pause_menu;
        Player.instance.exiting_door                    += unlink_pause_menu_toggle;
        Player.instance.exited_door                     += link_pause_menu_toggle;        
    }
    void unlink_player(){
        Player.instance.entered_door                    -= unlink_pause_menu_toggle;
        Player.instance.entered_door                    -= disable_pause_menu;
        Player.instance.exiting_door                    -= unlink_pause_menu_toggle;
        Player.instance.exited_door                     -= link_pause_menu_toggle;        
    }
    public void link_pause_menu_toggle(){
        InputManager.user_pause_performed += toggle_gameplay_ui;
        InputManager.menu_exit_performed += toggle_gameplay_ui;
    }
    public void unlink_pause_menu_toggle(){
        InputManager.user_pause_performed -= toggle_gameplay_ui;
        InputManager.menu_exit_performed -= toggle_gameplay_ui;
    }
}