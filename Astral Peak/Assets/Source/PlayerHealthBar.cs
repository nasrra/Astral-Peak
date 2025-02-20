using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthBar : MonoBehaviour{
    [SerializeField] List<HealthBarHeart> hearts;
    int health_amount = 0;

    void OnEnable(){
        if(Player.instance != null)
            set_health(Player.instance.get_health().get_current_health());
    }

    void Start(){
        Player.instance.intermediate_health_updated += intermediate_health_updated;
        Player.instance.get_health().healed         += health_updated;
        Player.instance.damaged_start               += health_updated;
        Player.instance.death_started               += dead;
        if(GameManager.get_state() != GameState.CUTSCENE)
            set_health(Player.instance.get_health().get_current_health());
    }
    void OnDestroy(){
        Player.instance.intermediate_health_updated -= intermediate_health_updated;
        Player.instance.get_health().healed         -= health_updated;
        Player.instance.damaged_start               -= health_updated;
        Player.instance.death_started               -= dead;
    }

    void health_updated() => set_health(Player.instance.get_health().get_current_health());
    void intermediate_health_updated() => set_intermediate_health(); 
    void dead() => set_health(0);

    public void set_health(int amt){
        if(gameObject.activeSelf == false)
            return;
        if(amt > hearts.Count)
            throw new System.Exception("Player health bar does not have: "+amt+" of notches " + hearts.Count);
        health_amount = amt-1;
        for(int i = 0; i < Player.instance.get_health().get_max_health(); i++){
            if(i>health_amount)
                hearts[i].disable();
            else
            hearts[i].enable();
            hearts[i].thump(i==health_amount);
        }
        set_intermediate_health();
    }

    public void set_intermediate_health(){
        for(int i = 0; i < Player.instance.get_health().get_max_health(); i++)
            hearts[i].set_fill(0);
        if(Player.instance.get_health().get_current_health() > 0 && Player.instance.get_health().get_current_health() < Player.instance.get_health().get_max_health()){
            for(int i = 0; i < Player.instance.get_health().get_max_health(); i++)
                hearts[i].set_fill(0);
            hearts[Player.instance.get_health().get_current_health()].set_fill(Player.instance.get_intermediate_health());
        }
    }

    public void fade_out(){
        if(gameObject.activeSelf == true)
            foreach(HealthBarHeart heart in hearts)
                heart.fade_out();
    }

    public void off(){
        if(gameObject.activeSelf == true)
            foreach(HealthBarHeart heart in hearts)
                heart.off();            
    }

    public void on(){
        if(gameObject.activeSelf == true)
            foreach(HealthBarHeart heart in hearts)
                heart.enable();            
    }

    public void fade_in(){
        set_health(Player.instance.get_health().get_current_health());
        if(gameObject.activeSelf == true)
            foreach(HealthBarHeart heart in hearts)
                heart.fade_in();
    } 
}