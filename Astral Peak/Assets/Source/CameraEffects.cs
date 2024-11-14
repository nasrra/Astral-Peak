using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CameraEffects : MonoBehaviour{
    static CameraEffectState
        none    = new CameraEffectState(0,0,0),
        normal  = new CameraEffectState(0.15f,0,0.35f),
        hurt    = new CameraEffectState(0.45f,-50,0.7f);
    [SerializeField] Volume volume;

    public void Start(){
        state_switch(normal,1);
        Player.player.damaged_start += hurt_state;
        Player.player.damaged_stop  += normal_state;
    }

    void OnDestroy(){
        reset_effects();
        Player.player.damaged_start -= hurt_state;
        Player.player.damaged_stop  -= normal_state;       
    }

    public void normal_state() => state_switch(normal, 3);
    
    public void hurt_state() => state_switch(hurt, 3);

    void state_switch(CameraEffectState state, float speed){
        handle_vignette(state.vignette_intensity, speed);
        handle_colour_adjustment(state.saturation_intensity, speed);
        handle_film_grain(state.film_grain_intensity, speed);
    }

    void handle_vignette(float intensity, float speed){
        volume.sharedProfile.TryGet(out Vignette vignette);
        StartCoroutine(vignette.intensity.value < intensity?
            increase_value(vignette.intensity, intensity, speed) :
            decrease_value(vignette.intensity, intensity, speed));    
    }

    void handle_colour_adjustment(float saturation, float speed){
        volume.sharedProfile.TryGet(out ColorAdjustments colour);
        StartCoroutine(colour.saturation.value < saturation? 
            increase_value(colour.saturation, saturation, speed * 100) : 
            decrease_value(colour.saturation, saturation, speed * 100));        
    }
    void handle_film_grain(float intensity, float speed){
        volume.sharedProfile.TryGet(out FilmGrain film_grain);
        StartCoroutine(film_grain.intensity.value < intensity? 
            increase_value(film_grain.intensity, intensity, speed) :
            decrease_value(film_grain.intensity, intensity, speed));     
    }     

    void instant_value_set(FloatParameter value, float n_value) => value.value = n_value;

    IEnumerator increase_value(FloatParameter value, float n_value, float time){
        while(value.value < n_value){
            value.value += Time.deltaTime * time;
            yield return null;
        }
        value.value = n_value;
        yield break;     
    }

    IEnumerator decrease_value(FloatParameter value, float n_value, float time){
        while(value.value > n_value){
            value.value -= Time.deltaTime * time;
            yield return null;
        }
        value.value = n_value;
        yield break;     
    }

    void reset_effects(){
        volume.sharedProfile.TryGet(out Vignette vignette);
        volume.sharedProfile.TryGet(out ColorAdjustments colour);
        volume.sharedProfile.TryGet(out FilmGrain film_grain);
        instant_value_set(vignette.intensity,   none.vignette_intensity);
        instant_value_set(colour.saturation,    none.saturation_intensity);
        instant_value_set(film_grain.intensity, none.film_grain_intensity);
    }
}

public struct CameraEffectState{
    public CameraEffectState(
        float _vignette_intensity,
        float _saturation_intensity,
        float _film_grain_intensity
    ){
        vignette_intensity      = _vignette_intensity;
        saturation_intensity    = _saturation_intensity;
        film_grain_intensity    = _film_grain_intensity;
    }
    
    public readonly float 
        vignette_intensity,
        saturation_intensity,
        film_grain_intensity;
}
