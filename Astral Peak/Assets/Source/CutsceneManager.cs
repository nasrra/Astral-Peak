using System;
using System.Collections;
using UnityEngine;

public static class CutsceneManager{
    public static Action<Cutscene> started_cutscene;
    static MonoBehaviour coroutines;
    public static void initialize(MonoBehaviour _coroutines) => coroutines = _coroutines;
    public static void play(Cutscene cutscene){
        GameManager.state_changed(GameState.CUTSCENE);
        started_cutscene?.Invoke(cutscene);
        coroutines.StartCoroutine(cutscene.get_coroutine());
        cutscene.ended += cutscene_ended;
    }
    static void cutscene_ended() => GameManager.state_changed(GameState.GAMEPLAY); 
}

public abstract class Cutscene{
    public event Action ended;
    public abstract IEnumerator get_coroutine();
    protected void end(){
        ended?.Invoke();
        ended = null;
    }
    protected void fade_to_black() =>   CameraEffects.instance.fade_to_black();
    protected void fade_from_black() => CameraEffects.instance.fade_from_black();
}
