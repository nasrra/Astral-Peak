using System;
using System.Collections;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CameraEffects : MonoBehaviour{
    public event Action screen_transition_completed;
    public static CameraEffects instance;
    [SerializeField] Volume volume;
    [SerializeField] Animator screen_transitions;
    [SerializeField] Coroutine fade_state;

    static Coroutine 
        vignette_state,
        colour_state,
        grain_state;

    public void Awake(){
        instance = this;
        state_switch(CameraEffectState.NORMAL);
    }

    void Start(){
        link_player();
    }

    void OnDestroy(){
        none_state();
        unlink_player();
        screen_transition_completed = null;
    }

    public void normal_state() => state_switch(CameraEffectState.NORMAL, 12);
    public void hurt_state() => state_switch(CameraEffectState.HURT, 12);
    public void flashback_state() => state_switch(CameraEffectState.FLASHBACK);
    public void none_state() => state_switch(CameraEffectState.NONE);

    void state_switch(CameraEffectState state, float speed){
        handle_vignette(state.vignette_intensity, speed);
        handle_colour_adjustment(state.saturation_intensity, speed);
        handle_film_grain(state.film_grain_intensity, speed);
        fade_to_colour(state.color);
    }

    void state_switch(CameraEffectState state){
        volume.sharedProfile.TryGet(out Vignette vignette);
        volume.sharedProfile.TryGet(out ColorAdjustments colour);
        volume.sharedProfile.TryGet(out FilmGrain film_grain);
        instant_value_set(vignette.intensity,   state.vignette_intensity);
        instant_value_set(colour.saturation,    state.saturation_intensity);
        instant_value_set(film_grain.intensity, state.film_grain_intensity);
        instant_colour_set(state.color);
    }

    void handle_vignette(float intensity, float speed){
        volume.sharedProfile.TryGet(out Vignette vignette);
        if(vignette_state != null)
            StopCoroutine(vignette_state);
        vignette_state = StartCoroutine(ValueHelper.lerp_value(vignette.intensity, intensity, speed));
    }

    void handle_colour_adjustment(float saturation, float speed){
        volume.sharedProfile.TryGet(out ColorAdjustments colour);
        if(colour_state != null)
            StopCoroutine(colour_state);
        colour_state = StartCoroutine(ValueHelper.lerp_value(colour.saturation, saturation, speed));        
    }
    void handle_film_grain(float intensity, float speed){
        volume.sharedProfile.TryGet(out FilmGrain film_grain);
        if(grain_state != null)
            StopCoroutine(grain_state);
        grain_state = StartCoroutine(ValueHelper.lerp_value(film_grain.intensity, intensity, speed));     
    }     

    public void fade_to_black(float time = 1){
        if(fade_state != null)
            StopCoroutine(fade_state);
        fade_state = StartCoroutine(screen_transition_coroutine("fade_to_black",time));
    }
    public void fade_from_black(float time = 1){
        if(fade_state != null)
            StopCoroutine(fade_state);
        fade_state = StartCoroutine(screen_transition_coroutine("fade_from_black",time));
    }
    IEnumerator screen_transition_coroutine(string transition, float time){
        screen_transitions.speed = time;
        screen_transitions.Play(transition);
        yield return new WaitForSeconds(time);
        screen_transition_completed?.Invoke();
    }

    public void instant_value_set(FloatParameter value, float n_value) => value.value = n_value;

    public void instant_colour_set(Color _colour){
        volume.sharedProfile.TryGet(out ColorAdjustments colour);
        colour.colorFilter.value = _colour;
    }

    public void fade_to_colour(Color color){
        volume.sharedProfile.TryGet(out ColorAdjustments colour);
        StartCoroutine(ValueHelper.lerp_colours(colour.colorFilter, color, 1f));
    }

    void link_player(){
        if(Player.instance == null){
            Debug.Log("no player!");
            return;
        }
        Player.instance.damaged_start += hurt_state;
        Player.instance.damaged_stop  += normal_state;
        Player.instance.death_start   += hurt_state;
        Player.instance.death         += normal_state; 
    }

    void unlink_player(){
        if(Player.instance == null){
            //Debug.Log("no player!");
            return;
        }
        Player.instance.damaged_start -= hurt_state;
        Player.instance.damaged_stop  -= normal_state;  
        Player.instance.death_start   -= hurt_state;  
        Player.instance.death         -= normal_state;    
    }
}

public struct CameraEffectState{
    public CameraEffectState(
        float _vignette_intensity,
        float _saturation_intensity,
        float _film_grain_intensity,
        Color _color
    ){
        vignette_intensity      = _vignette_intensity;
        saturation_intensity    = _saturation_intensity;
        film_grain_intensity    = _film_grain_intensity;
        color = _color;
    }

    public readonly static CameraEffectState 
    NONE        = new CameraEffectState(0,0,0, Color.white),
    NORMAL      = new CameraEffectState(0.05f,0,0.25f, Color.white),
    HURT        = new CameraEffectState(0.35f,-25,0.45f, Color.white),
    FLASHBACK   = new CameraEffectState(.55f, -100, 1, Color.white);

    public readonly float 
        vignette_intensity,
        saturation_intensity,
        film_grain_intensity;
    
    public readonly Color
        color;
}
