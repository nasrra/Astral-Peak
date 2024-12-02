using System.Collections;
using System.Collections.Generic;
using DocumentFormat.OpenXml.Presentation;
using Unity.VisualScripting;
using UnityEngine;

public class ShrineCutsceneTorch : MonoBehaviour{
    [SerializeField] List<Torch> torches = new List<Torch>();
    AudioSource source;
    void OnEnable(){
        ShrineCutscene.torches_on   += turn_on;
        ShrineCutscene.torches_off  += turn_off;
        ShrineCutscene.enlargen_torches += enlargen;
        ShrineCutscene.reset_torches += reset_torches;
        Application.quitting += OnDisable;
    }

    void OnDisable(){
        ShrineCutscene.torches_on   -= turn_on;
        ShrineCutscene.torches_off  -= turn_off;
        ShrineCutscene.enlargen_torches -= enlargen;
        ShrineCutscene.reset_torches -= reset_torches;
        Application.quitting        -= OnDisable;
    }

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
}
