using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SceneLighting : LightingHandler{
    public static SceneLighting instance;
    [Header("Scene Lighting")]
    [SerializeField] SerializedDictionary<string, SceneLightingPreset> presets = new SerializedDictionary<string, SceneLightingPreset>();
    int current_preset = 0;
    
    void Awake(){
        instance = this;
        foreach(KeyValuePair<string, Light2D> kvp in lights)
            set_preset(kvp.Value, presets[format_preset_id(kvp.Key)]);
    }
    public void reset_lighting(string _id, float _time) => lerp_preset(_id, current_preset, _time);
    public string format_preset_id(string id) => current_preset+"_"+id; 

    public void lerp_preset(string _id, int _preset, float time){
        Light2D light = lights[_id];
        current_preset = _preset;
        SceneLightingPreset preset = presets[format_preset_id(_id)];
        light.StopAllCoroutines();
        lerp_intensity(light: light, start: light.intensity, end: preset.intensity, time: time);
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
