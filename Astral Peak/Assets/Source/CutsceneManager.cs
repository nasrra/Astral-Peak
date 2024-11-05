using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CutsceneManager : MonoBehaviour{
    public static CutsceneManager instance;
    Coroutine state;
    
    public void Awake() => instance = this;
    
    public void Play(string cutscene_id){
        Cutscene cutscene = CutsceneLibrary.create_cutscene[cutscene_id]();
        cutscene.begin();
    }

    public void switch_state(IEnumerator n_state){
        if(state != null)
            StopCoroutine(state);
        state = StartCoroutine(n_state);
    }
}

public abstract class Cutscene{
    public abstract void begin();
    public abstract void end();
}

public static class CutsceneLibrary{
    public delegate Cutscene CutsceneCreation();
    public readonly static Dictionary<string, CutsceneCreation> create_cutscene = new Dictionary<string, CutsceneCreation>(){
        {"test",()=>new CutsceneTest()},
    };
}
