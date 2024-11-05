using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManager : MonoBehaviour{
    public event Action
        death_screen_ended;
    public static UiManager instance;
    [SerializeField] Canvas ui;
    [SerializeField] Animator animator;
    
    void Awake(){
        instance = this;
        GameManager.link_Ui();
    }

    void OnDestroy() => GameManager.unlink_Ui();

    public void play_death_screen(){
        animator.Play("death_screen");
    }
    public void death_screen_end() => death_screen_ended?.Invoke();
}
