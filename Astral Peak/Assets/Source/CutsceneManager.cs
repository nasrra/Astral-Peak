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
        cutscene.ended += cutscene_ended;
    }
    static void cutscene_ended() => GameManager.state_changed(GameState.GAMEPLAY);
}

public abstract class Cutscene{
    public event Action ended;
    protected float fade_transition_time = 1;
    public abstract IEnumerator get_coroutine();
    protected void end(){
        ended?.Invoke();
        ended = null;
    }
}
