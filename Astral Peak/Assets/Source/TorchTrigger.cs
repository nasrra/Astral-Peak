using System.Collections.Generic;
using UnityEngine;

public class TorchTrigger : MonoBehaviour{
    [SerializeField] List<Torch> 
    on = new List<Torch>(),
    off = new List<Torch>();

    void OnTriggerEnter2D(){
        foreach(Torch t in on)
            t.turn_on();
        foreach(Torch t in off)
            t.turn_off();
    }
}
