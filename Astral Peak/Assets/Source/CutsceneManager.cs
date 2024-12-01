using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CutsceneManager{
    static MonoBehaviour coroutines;
    static Cutscene cutscene;
    public static void initialize(MonoBehaviour _coroutines) => coroutines = _coroutines;
    public static void play(string cutscene_id){
        UiManager.instance.cutscene_mode(true);
        cutscene = CutsceneLibrary.create_cutscene[cutscene_id]();
        cutscene.start();
        cutscene.ended += cutscene_ended;
    }
    static void cutscene_ended(){
        UiManager.instance.cutscene_mode(false);
        cutscene.ended -= cutscene_ended;
        cutscene = null;
    }
    public static void set_coroutine(IEnumerator c){
        coroutines.StartCoroutine(c);
    }
    public static void invoke_event(Action action) => action?.Invoke();
}

public abstract class Cutscene{
    public event Action ended;
    public abstract void start();
    protected void end() => ended?.Invoke();
    protected void fade_to_black() => UiManager.instance.fade_to_black();
    protected void fade_from_black() => UiManager.instance.fade_from_black();
}

public static class CutsceneLibrary{
    public readonly static Dictionary<string, Func<Cutscene>> create_cutscene = new Dictionary<string, Func<Cutscene>>(){
        {"shrine_cutscene",()=>new ShrineCutscene()},
        {"cavalry_transition_1",()=>new CavalryPhaseTransition()},
        {"cavalry_opening",     ()=> new CavalryOpeningCutscene()},
    };
}
