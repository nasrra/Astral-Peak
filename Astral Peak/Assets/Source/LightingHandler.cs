using System;
using AYellowpaper.SerializedCollections;
using Entropek;
using UnityEngine;
using UnityEngine.Rendering.Universal;


public class LightingHandler : MonoBehaviour{
    [Header("Lighting Handler")]
    [SerializeField] protected SerializedDictionary<string, Light2D> lights = new SerializedDictionary<string, Light2D>();
    
    public void enable_light(string id, bool enable) => lights[id].enabled = enable;
    
    public void set_intensity(float value){
        foreach(Light2D light in lights.Values)
            light.intensity = value;
    }
    public void set_intensity(string id, float value) => lights[id].intensity = value;
    protected void set_intensity(Light2D light, float value) => light.intensity = value;
    
    public void lerp_intensity(string id, float end, float time, float? start = null,  Action callback = null){
        Light2D light = lights[id];
        light.StopAllCoroutines();
        lerp_intensity(light, start??light.intensity, end, time, callback);
    }
    protected void lerp_intensity(Light2D light, float start, float end, float time, Action callback = null) =>
        light.StartCoroutine(Calc.lerp_value(
            _val=>light.intensity=_val,
            _start:start,
            _end:end,
            _time:time,
            _on_complete: ()=>callback?.Invoke()));
    //protected void pulse_intensity(string id, int pulses = 1, float intensity, float time = 0.35f){
    //    
    //}
}
