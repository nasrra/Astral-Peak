using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthBar : MonoBehaviour
{
    [SerializeField] List<HealthBarHeart> hearts;

    void OnEnable(){
        if(Player.instance != null)
            set_health(Player.instance.get_health().get_current_health());
    }

    void Start(){
        Player.instance.get_health().healed += health_updated;
        Player.instance.damaged_start       += health_updated;
        Player.instance.death_started       += dead;
        set_health(Player.instance.get_health().get_current_health());
        fade_in();
    }
    void OnDestroy(){
        Player.instance.get_health().healed -= health_updated;
        Player.instance.damaged_start       -= health_updated;
        Player.instance.death_started       -= dead;
    }

    void health_updated() => set_health(Player.instance.get_health().get_current_health()); 
    void dead() => set_health(0);

    public void set_health(int amt){
        if(gameObject.activeSelf == false)
            return;
        if(amt > hearts.Count)
            throw new System.Exception("Player health bar does not have: "+amt+" of notches " + hearts.Count);
        int a = amt-1;
        for(int i = 0; i < Player.instance.get_health().get_max_health(); i++){
            if(i>a)
                hearts[i].disable();
            else
            hearts[i].enable();
            hearts[i].thump(i==a);
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

    public void fade_in(){
        if(gameObject.activeSelf == true)
            foreach(HealthBarHeart heart in hearts)
                heart.fade_in();
    } 
}