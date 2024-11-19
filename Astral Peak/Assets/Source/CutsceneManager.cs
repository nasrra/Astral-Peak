using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CutsceneManager{
    static MonoBehaviour coroutines;
    public static void initialize(MonoBehaviour _coroutines) => coroutines = _coroutines;
    public static void play(string cutscene_id){
        Cutscene cutscene = CutsceneLibrary.create_cutscene[cutscene_id]();
        cutscene.begin();
    }
    public static void set_coroutine(IEnumerator c){
        coroutines.StartCoroutine(c);
    }
}

public abstract class Cutscene{
    public abstract void begin();
    public abstract void end();
    protected void fade_to_black() => CameraEffects.instance.fade_to_colour(Color.black);
    protected void fade_from_black() => CameraEffects.instance.fade_to_colour(Color.white);
}

public static class CutsceneLibrary{
    public readonly static Dictionary<string, Func<Cutscene>> create_cutscene = new Dictionary<string, Func<Cutscene>>(){
        {"test",                ()=>new CutsceneTest()},
        {"cavalry_transition_1",()=>new CavalryPhaseTransition()},
        {"cavalry_opening",     ()=> new CavalryOpeningCutscene()},
    };
}
