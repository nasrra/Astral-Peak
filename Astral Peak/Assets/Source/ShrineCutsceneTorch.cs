using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrineCutsceneTorch : MonoBehaviour{
    [SerializeField] List<Torch> torches = new List<Torch>();
    AudioSource source;
    void OnEnable() => link();
    void OnDisable() => unlink();

    void turn_on() => StartCoroutine(turn_on_coroutine());
    IEnumerator turn_on_coroutine(){
        foreach(Torch t in torches){
            t.turn_on();
            yield return new WaitForSeconds(1f);
            yield return null;
        }
    }

    void turn_off() => StartCoroutine(turn_off_coroutine());
    IEnumerator turn_off_coroutine(){
        foreach(Torch t in torches){
            t.turn_off();
            yield return new WaitForSeconds(.5f);
            yield return null;
        }
    }

    void enlargen(){
        foreach(Torch t in torches)
            t.enlargen();
    }

    void reset_torches(){
        foreach(Torch t in torches)
            t.revert();  
    }

    void handle_cutscene(Cutscene cutscene){
        switch(cutscene){
            case ShrineAltarCutscene c:
                c.torches_on += turn_on;                
                break;
            case ShrineOpeningCutscene c:
                c.torches_on        += turn_on;
                c.torches_off       += turn_off;
                c.enlargen_torches  += enlargen;
                c.reset_torches     += reset_torches;
                break;
        }
    }

    void link() => CutsceneManager.started_cutscene += handle_cutscene;
    void unlink() => CutsceneManager.started_cutscene -= handle_cutscene;
}
