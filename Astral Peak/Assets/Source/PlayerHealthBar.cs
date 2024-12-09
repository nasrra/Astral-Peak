using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthBar : MonoBehaviour
{
    [SerializeField] List<HealthBarHeart> hearts;

    void Start(){
        Player.instance.get_health().healed += health_updated;
        Player.instance.damaged_start       += health_updated;
        Player.instance.death_start         += dead;
        set_health(Player.instance.get_health().get_current_health());
        fade_in();
    }
    void OnDestroy(){
        Player.instance.get_health().healed -= health_updated;
        Player.instance.damaged_start       -= health_updated;
        Player.instance.death_start         -= dead;
    }

    void health_updated() => set_health(Player.instance.get_health().get_current_health()); 
    void dead() => set_health(0);

    public void set_health(int amt){
        Debug.Log(gameObject.name);
        if(amt > hearts.Count)
            throw new System.Exception("Player health bar does not have: "+amt+" of notches " + hearts.Count);
        int a = amt-1;
        for(int i = 0; i < Player.instance.get_health().get_max_health(); i++){
            if(i>a)
                hearts[i].turn_off();
            else
                hearts[i].turn_on();
            hearts[i].thump(i==a);
        }
    }

    public void fade_out(){
        foreach(HealthBarHeart heart in hearts)
            heart.fade_out();
    }

    public void fade_in(){
        foreach(HealthBarHeart heart in hearts)
            heart.fade_in();
    } 
}