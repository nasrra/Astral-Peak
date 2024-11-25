using System;
using System.Collections;
using UnityEngine;

public class UiManager : MonoBehaviour{
    public event Action
        death_screen_ended;
    public static UiManager instance;
    [SerializeField] GameObject 
        death_screen,
        settings_menu,
        hud,
        enemy_vanquished;
    [SerializeField] Animator
        screen_transitions;
    AudioSource source;
    
    void Awake(){
        instance = this;
        GameManager.link_Ui();
    }

    void Start() => enable_hud();

    void disable_all(){
        death_screen.SetActive(false);
        settings_menu.SetActive(false);
        hud.SetActive(false);
        InputManager.exit_performed -= enable_hud;
        InputManager.exit_performed -= enable_settings_menu;
    }

    public void enable_settings_menu(){
        disable_all();
        settings_menu.SetActive(true);
        InputManager.exit_performed += enable_hud;
    }

    public void enable_hud(){
        disable_all();
        hud.SetActive(true);
        InputManager.exit_performed += enable_settings_menu;
    }

    public void enable_death_screen(){
        disable_all();
        play_death_screen();
    }

    public void play_enemy_vanquished() => StartCoroutine(enemy_vanquished_state());
    IEnumerator enemy_vanquished_state(){
        AudioClipHandler.play(
            SoundID.WOODEN_PING,
            audio_player:       this, 
            source:             out source, 
            randomise_pitch:    false, 
            spatial_blend:      false);
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
            spatial_blend:      false);
        death_screen.SetActive(true);
        yield return new WaitForSeconds(4);
        UiManager.instance.fade_to_black();
        yield return new WaitForSeconds(4);  
        death_screen_ended?.Invoke();
        yield break;
    }

    public void fade_to_black() => screen_transitions.Play("fade_to_black");
    public void fade_from_black() => screen_transitions.Play("fade_from_black");
}