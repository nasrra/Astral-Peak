using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthBar : MonoBehaviour{
    [SerializeField] List<GameObject> notches = new List<GameObject>();

    void Start(){
        Player.instance.damaged_start += player_damaged;
        Player.instance.death_start += player_damaged;
    }
    void OnDestroy(){
        Player.instance.damaged_start -= player_damaged;
        Player.instance.death_start -= player_damaged;
    }

    void player_damaged() => set_health(Player.instance.get_health().get_current_health());

    public void set_health(int amt){
        if(amt > notches.Count)
            throw new System.Exception("Player health bar does not have: "+amt+" of notches");
        int a = amt-1;
        for(int i = 0; i <= amt; i++)
            notches[i].SetActive(i>a?false:true);
    }
}
