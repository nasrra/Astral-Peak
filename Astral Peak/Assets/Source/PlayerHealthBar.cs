using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthBar : MonoBehaviour{
    Health health;
    [SerializeField] List<HealthBarHeart> hearts;
    int health_amount = 0;

    //void OnEnable(){
    //    if(Player.instance != null){
    //        health = Player.instance.health;
    //        set_hearts();
    //    }
    //}

    void Start(){
        health = Player.instance.health;
        set_hearts();
        Player.instance.intermediate_health_updated += intermediate_health_updated;
        Player.instance.intermediate_health_gained  += intermediate_health_gained;
        health.healed                               += health_updated;
        Player.instance.damaged_start               += health_updated;
        Player.instance.death_started               += dead;
        if(GameManager.get_state() != GameState.CUTSCENE)
            set_hearts();
    }
    void OnDestroy(){
        Player.instance.intermediate_health_updated -= intermediate_health_updated;
        health.healed                               -= health_updated;
        Player.instance.damaged_start               -= health_updated;
        Player.instance.death_started               -= dead;
    }

    void health_updated() => set_hearts();
    void intermediate_health_updated(){
        set_intermediate_health();
    } 
    void dead() => set_hearts();

    public void set_hearts(){
        if(gameObject.activeSelf == false)
            return;
        health_amount = health.get_current_health()-1;
        for(int i = 0; i < health.get_max_health(); i++){
            if(i>health_amount)
                hearts[i].disable();
            else
                hearts[i].enable();
            hearts[i].thump(i==health_amount);
        }
        set_intermediate_health();
    }

    public void set_intermediate_health(){
        for(int i = 0; i < hearts.Count; i++)
            hearts[i].set_fill(i==health.get_current_health()?Player.instance.get_intermediate_health():0);
    }

    public void intermediate_health_gained(){
        hearts[health.get_current_health()].health_gained_flash();
        set_intermediate_health();
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
        set_hearts();
        if(gameObject.activeSelf == true)
            foreach(HealthBarHeart heart in hearts)
                heart.fade_in();
    } 
}