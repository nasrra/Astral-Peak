using System;
using System.Collections;
using Entropek;
using UnityEngine;

public static class CutsceneManager{
    public static readonly float skip_buffer_time = 1f;
    static Coroutine skip_state;
    public static Action<Cutscene> started_cutscene;
    static MonoBehaviour coroutines;
    private static Cutscene cutscene = null;
    public static void initialize(MonoBehaviour _coroutines) => coroutines = _coroutines;
    public static void play(Cutscene _cutscene){
        GameManager.state_changed(GameState.CUTSCENE);
        cutscene = _cutscene;
        started_cutscene?.Invoke(cutscene);
        set_coroutine(cutscene.get_coroutine());
        cutscene.ended += cutscene_ended;
        InputManager.cutscene_skip_performed += start_skip;
        InputManager.cutscene_skip_canceled += stop_skip;
    }
    public static bool in_cutscene() => cutscene != null;

    public static void set_coroutine(IEnumerator coroutine) => coroutines.StartCoroutine(coroutine); 
    static void cutscene_ended(){
        Log.MethodCall();
        cutscene = null;
        GameManager.state_changed(GameState.GAMEPLAY);
        Time.timeScale = 1;
        InputManager.cutscene_skip_performed -= start_skip;
        InputManager.cutscene_skip_canceled -= stop_skip;
    }

    static void start_skip()
        => skip_state = UnityHook.instance.StartCoroutine(Util.timer(
            skip_buffer_time,
            time_out: ()=> skip_cutscene()
        ));
    static void stop_skip(){
        if(skip_state!=null)
            UnityHook.instance.StopCoroutine(skip_state);
    }

    static void skip_cutscene(){
        Time.timeScale = 100;
        InputManager.cutscene_skip_performed -= start_skip;
        InputManager.cutscene_skip_canceled -= stop_skip;
    }
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
