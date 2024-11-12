using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManager : MonoBehaviour{
    public event Action
        death_screen_ended;
    public static UiManager instance;
    [SerializeField] GameObject 
        death_screen,
        settings_menu,
        hud;

    void Awake(){
        instance = this;
        GameManager.link_Ui();
    }

    void Start() => enable_hud();

    void disable_all(){
        death_screen.SetActive(false);
        settings_menu.SetActive(false);
        hud.SetActive(false);
        InputManager.instance.exit_performed -= enable_hud;
        InputManager.instance.exit_performed -= enable_settings_menu;
    }

    public void enable_settings_menu(){
        disable_all();
        settings_menu.SetActive(true);
        InputManager.instance.exit_performed += enable_hud;
    }

    public void enable_hud(){
        disable_all();
        hud.SetActive(true);
        InputManager.instance.exit_performed += enable_settings_menu;
    }

    public void enable_death_screen(){
        disable_all();
        death_screen.SetActive(true);
        StartCoroutine(death_screen_timer());
    }

    IEnumerator death_screen_timer(){
        yield return new WaitForSeconds(3);
        death_screen_ended?.Invoke();
    }
}