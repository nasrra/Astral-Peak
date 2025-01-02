using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using Deluz;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SceneLighting : MonoBehaviour{
    public static SceneLighting instance;
    [SerializedDictionary("id","Light2D")]
    [SerializeField] SerializedDictionary<string, Light2D> lights = new SerializedDictionary<string, Light2D>();
    [SerializeField] SerializedDictionary<string, SceneLightingPreset> presets = new SerializedDictionary<string, SceneLightingPreset>();
    int current_preset = 0;
    void Awake(){
        instance = this;
        foreach(KeyValuePair<string, Light2D> kvp in lights)
            set_preset(kvp.Value, presets[format_preset_id(kvp.Key)]);
    }
    public string format_preset_id(string id) => current_preset+"_"+id; 
    public void reset_lighting(string _id, float _time) => lerp_preset(_id, current_preset, _time);
    public void enable_light(string id, bool enable) => lights[id].enabled = enable;
    public void set_intensity(string id, float value) => lights[id].intensity = value;
    private void set_intensity(Light2D light, float value) => light.intensity = value;
    public void lerp_intensity(string id, float value, float time, Action callback = null){
        Light2D light = lights[id];
        light.StopAllCoroutines();
        lerp_intensity(light, value, time, callback);
    }
    private void lerp_intensity(Light2D light, float value, float time, Action callback = null) =>
        light.StartCoroutine(Calc.lerp_value(
            _val=>light.intensity=_val,
            _start:light.intensity,
            _end:value,
            _time:time,
            _on_complete: ()=>callback?.Invoke()));

    public void lerp_preset(string _id, int _preset, float time){
        Light2D light = lights[_id];
        current_preset = _preset;
        SceneLightingPreset preset = presets[format_preset_id(_id)];
        light.StopAllCoroutines();
        lerp_intensity(light, preset.intensity, time);
    }
    public void set_preset(string _id, int _preset){
        current_preset = _preset;
        set_preset(lights[_id], presets[format_preset_id(_id)]);
    }
    private void set_preset(Light2D light, SceneLightingPreset preset){
        light.StopAllCoroutines();
        set_intensity(light, preset.intensity);
    }
}

[Serializable]
struct SceneLightingPreset{
    public float intensity;
    public SceneLightingPreset(float _intensity){
        intensity = _intensity;
    }
}
