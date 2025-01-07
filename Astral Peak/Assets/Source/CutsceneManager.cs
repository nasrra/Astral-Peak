using System;
using System.Collections;
using UnityEngine;

public static class CutsceneManager{
    public static Action<Cutscene> started_cutscene;
    static MonoBehaviour coroutines;
    private static Cutscene cutscene;
    public static void initialize(MonoBehaviour _coroutines) => coroutines = _coroutines;
    public static void play(Cutscene _cutscene){
        GameManager.state_changed(GameState.CUTSCENE);
        cutscene = _cutscene;
        started_cutscene?.Invoke(cutscene);
        coroutines.StartCoroutine(cutscene.get_coroutine());
        InputManager.skip_cutscene_performed += cutscene.skip;
        cutscene.ended += cutscene_ended;
    }
    static void cutscene_ended(){
        InputManager.skip_cutscene_performed -= cutscene.skip;
        GameManager.state_changed(GameState.GAMEPLAY);
    }  
}

public abstract class Cutscene{
    public event Action ended;
    public abstract IEnumerator get_coroutine();
    protected void end(){
        ended?.Invoke();
        ended = null;
    }
    protected void fade_to_black() =>   CameraEffects.instance.fade_to_black(1);
    protected void fade_from_black() => CameraEffects.instance.fade_from_black(1);
    public abstract void skip();
}
