using System.Collections.Generic;
using UnityEngine;

public class ShrineCutsceneTorch : MonoBehaviour{
    [SerializeField] List<Torch> torches = new List<Torch>();
    void Awake(){
        foreach(Torch t in torches){
            ShrineCutscene.torches_on += t.turn_on;
            ShrineCutscene.torches_off += t.turn_off;
        }
    }

    void OnDestroy(){
        foreach(Torch t in torches){
            ShrineCutscene.torches_on -= t.turn_on;
            ShrineCutscene.torches_off -= t.turn_off;
        }
    }
}
