using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CutsceneManager{
    public static Action<Cutscene> started_cutscene;
    static MonoBehaviour coroutines;
    static Cutscene cutscene;
    public static void initialize(MonoBehaviour _coroutines) => coroutines = _coroutines;
    public static void play(string cutscene_id){
        GameManager.state_changed(GameState.CUTSCENE);
        cutscene = CutsceneLibrary.create_cutscene[cutscene_id]();
        started_cutscene?.Invoke(cutscene);
        cutscene.start();
        cutscene.ended += cutscene_ended;
    }
    static void cutscene_ended(){
        GameManager.state_changed(GameState.GAMEPLAY); 
        cutscene.ended -= cutscene_ended;
        cutscene = null;
    }
    public static void set_coroutine(IEnumerator c){
        coroutines.StartCoroutine(c);
    }
}

public abstract class Cutscene{
    public event Action ended;
    public abstract void start();
    protected void end() => ended?.Invoke();
    protected void fade_to_black() =>   CameraEffects.instance.fade_to_black();
    protected void fade_from_black() => CameraEffects.instance.fade_from_black();
}

public static class CutsceneLibrary{
    public readonly static Dictionary<string, Func<Cutscene>> create_cutscene = new Dictionary<string, Func<Cutscene>>(){
        {"shrine_opening",()=>new ShrineOpeningCutscene()},
        {"shrine_altar_1",()=>new ShrineAltarOneCutscene()},
        {"cavalry_transition_1",()=>new CavalryPhaseTransition()},
        {"cavalry_opening",     ()=> new CavalryOpeningCutscene()},
    };
}//
