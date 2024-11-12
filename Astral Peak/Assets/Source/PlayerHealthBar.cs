using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthBar : MonoBehaviour{
    public static PlayerHealthBar instance;
    [SerializeField] List<GameObject> notches = new List<GameObject>();

    void Awake() => instance = this; 

    public void set_health(int amt){
        if(amt > notches.Count)
            throw new System.Exception("Player health bar does not have: "+amt+" of notches");
        int a = amt-1;
        for(int i = 0; i <= amt; i++)
            notches[i].SetActive(i>a?false:true);
    }
}
