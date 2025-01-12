using System;
using System.Collections;
using Entropek;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CameraEffects : MonoBehaviour{
    public event Action 
        started_fade_to_black,
        completed_fade_to_black,
        started_fade_from_black,
        completed_fade_from_black;
    public static CameraEffects instance;
    [SerializeField] Volume volume;
    [SerializeField] Animator screen_transitions;
    [SerializeField] Coroutine fade_state;

    [SerializeField] Vignette vignette;
    [SerializeField] ColorAdjustments colour;
    [SerializeField] FilmGrain film_grain;


    static Coroutine 
        vignette_state,
        
        colour_saturation_state,
        colour_state,
        grain_state;

    public void Awake(){
        instance = this;
        get_volume_components();
        set_preset(CameraEffectState.NORMAL);
    }

    void Start(){
        link_player();
    }

    void OnDestroy(){
        none_state();
        // unlink for Door and BossRoom cutscene transitions, and scene transitions.
        started_fade_to_black = null;
        completed_fade_to_black = null;
        started_fade_from_black = null;
        completed_fade_from_black = null;
    }

    public void normal_state()      => lerp_preset(CameraEffectState.NORMAL,    .35f);
    public void hurt_state()        => lerp_preset(CameraEffectState.HURT,      .35f);
    public void flashback_state()   => set_preset(CameraEffectState.FLASHBACK);
    public void none_state()        => set_preset(CameraEffectState.NONE);

    void state_switch(ref Coroutine state, IEnumerator _state){
        if(state!=null)
            StopCoroutine(state);
        state = _state!=null? StartCoroutine(_state) : null;
    }

    void lerp_preset(CameraEffectState preset, float time){
        state_switch(ref vignette_state, Calc.lerp_value(val=>vignette.intensity.value=val, _start:vignette.intensity.value, _end: preset.vignette_intensity, _time:time));
        state_switch(ref colour_saturation_state, Calc.lerp_value(val=>colour.saturation.value=val, _start:colour.saturation.value, _end: preset.colour_saturation, _time:time));
        state_switch(ref grain_state, Calc.lerp_value(val=>film_grain.intensity.value=val, _start:film_grain.intensity.value, _end: preset.film_grain_intensity, _time:time));
        state_switch(ref colour_state, Calc.lerp_color(val=>colour.colorFilter.value=val, _start:colour.colorFilter.value, _end: preset.colour_filter, _time:time));
    }

    void set_preset(CameraEffectState state){
        vignette.intensity.value   = state.vignette_intensity;
        colour.saturation.value    = state.colour_saturation;
        film_grain.intensity.value = state.film_grain_intensity;
        colour.colorFilter.value   = state.colour_filter;
    }

    void get_volume_components(){
        volume.sharedProfile.TryGet(out Vignette _vignette);
        vignette = _vignette;
        volume.sharedProfile.TryGet(out ColorAdjustments _colour);
        colour = _colour;
        volume.sharedProfile.TryGet(out FilmGrain _film_grain);
        film_grain = _film_grain;
    }

    public void fade_to_black(float time, Action time_out = null) 
        => state_switch(ref fade_state, screen_transition_coroutine(
            transition: "fade_to_black",
            time: time, 
            start_action: started_fade_to_black, 
            time_out: ()=>{
                time_out?.Invoke();
                completed_fade_to_black?.Invoke();
            }));
    public void fade_from_black(float time, Action time_out = null) 
        => state_switch(ref fade_state, screen_transition_coroutine(
            transition: "fade_from_black",
            time: time, 
            start_action: started_fade_from_black, 
            time_out: ()=>{
                time_out?.Invoke();
                completed_fade_from_black?.Invoke();
            }));
    IEnumerator screen_transition_coroutine(string transition, float time, Action start_action, Action time_out) =>
        Util.timer(
            time: time,
            start_action:()=>{
                screen_transitions.speed = time;
                screen_transitions.Play(transition);
                start_action?.Invoke();                
            },
            time_out:()=>time_out?.Invoke()
        );

    void link_player(){
        if(Player.instance == null){
            Debug.Log("no player!");
            return;
        }
        Player.instance.on_destroy      += unlink_player;
        Player.instance.damaged_start   += hurt_state;
        Player.instance.damaged_stop    += normal_state;
        Player.instance.death_started   += hurt_state;
        Player.instance.death_completed += normal_state; 
    }

    void unlink_player(){
        if(Player.instance == null){
            Debug.Log("no player!");
            return;
        }
        Player.instance.on_destroy      -= unlink_player;
        Player.instance.damaged_start   -= hurt_state;
        Player.instance.damaged_stop    -= normal_state;  
        Player.instance.death_started   -= hurt_state;  
        Player.instance.death_completed -= normal_state;    
    }
}

public struct CameraEffectState{
    public readonly float 
        vignette_intensity,
        colour_saturation,
        film_grain_intensity;
    
    public readonly Color
        colour_filter;
    public CameraEffectState(
        float _vignette_intensity,
        float _colour_saturation,
        float _film_grain_intensity,
        Color _colour_filter
    ){
        vignette_intensity      = _vignette_intensity;
        colour_saturation       = _colour_saturation;
        film_grain_intensity    = _film_grain_intensity;
        colour_filter = _colour_filter;
    }
    public readonly static CameraEffectState 
    NONE        = new CameraEffectState(0,0,0, Color.white),
    NORMAL      = new CameraEffectState(0.05f,0,0.25f, Color.white),
    HURT        = new CameraEffectState(0.35f,-25,0.45f, Color.white),
    FLASHBACK   = new CameraEffectState(.45f, -100, 1, Color.white);
}
