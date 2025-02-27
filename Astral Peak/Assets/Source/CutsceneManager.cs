using System;
using System.Collections;
using Entropek;
using UnityEngine;

public static class CutsceneManager{
    public static readonly float skip_buffer_time = 1f;
    static Coroutine skip_state;
    static bool skipping = false;
    public static Action<Cutscene> started_cutscene;
    static MonoBehaviour coroutines;
    private static Cutscene cutscene = null;
    public static void initialize(MonoBehaviour _coroutines){
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.playModeStateChanged += handle_play_mode_state_changed;
        #endif 
        coroutines = _coroutines;
    }
    public static void play(Cutscene _cutscene){
        GameManager.state_changed(GameState.CUTSCENE);
        cutscene = _cutscene;
        started_cutscene?.Invoke(cutscene);
        set_coroutine(cutscene.get_coroutine());
        cutscene.ended          += cutscene_ended;
        cutscene.stopped_skip   += stop_skip_cutscene;
        link_cutscene_skip();
    }
    public static bool in_cutscene() => cutscene != null;

    public static void set_coroutine(IEnumerator coroutine) => coroutines.StartCoroutine(coroutine); 
    static void cutscene_ended(){
        cutscene.ended -= cutscene_ended;
        cutscene = null;
        GameManager.state_changed(GameState.GAMEPLAY);
    }

    static void start_skip_cutscene(){
        skipping = true;
        GameManager.set_time_scale(80);
        unlink_cutscene_skip();
    }

    static void stop_skip_cutscene(){
        cutscene.stopped_skip -= stop_skip_cutscene;
        GameManager.set_time_scale(1);
        unlink_cutscene_skip();
        skipping = false;
    }

    static void start_skip_input_performed(){
        skip_state = UnityHook.instance.StartCoroutine(Util.timer(
            skip_buffer_time,
            time_out: ()=> start_skip_cutscene()
        ));
    }
    static void start_skip_input_cancelled(){
        if(skip_state!=null){
            UnityHook.instance.StopCoroutine(skip_state);
            skip_state = null;
        }
    }
    public static bool is_skipping(){
        return skipping;
    }
    
    private static void link_cutscene_skip(){
        InputManager.cutscene_skip_performed += start_skip_input_performed;
        InputManager.cutscene_skip_canceled  += start_skip_input_cancelled;        
    }

    private static void unlink_cutscene_skip(){
        InputManager.cutscene_skip_performed -= start_skip_input_performed;
        InputManager.cutscene_skip_canceled  -= start_skip_input_cancelled;        
    }

    public static Cutscene get_cutscene() => cutscene;

    #if UNITY_EDITOR
    private static void handle_play_mode_state_changed(UnityEditor.PlayModeStateChange state){
        if (state == UnityEditor.PlayModeStateChange.ExitingPlayMode){
            UnityEditor.EditorApplication.playModeStateChanged -= handle_play_mode_state_changed;
            unlink_cutscene_skip();
        }
    }
    #endif
}

public abstract class Cutscene{
    public event Action ended, stopped_skip;
    protected float fade_transition_time = 1;
    public abstract IEnumerator get_coroutine();
    protected void end(){
        ended?.Invoke();
        ended = null;
    }
    protected void stop_skip(){
        stopped_skip?.Invoke();
        stopped_skip = null;
    }
}
