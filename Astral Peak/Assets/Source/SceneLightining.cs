using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using Deluz;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SceneLightining : MonoBehaviour{
    public static SceneLightining instance;
    [SerializedDictionary("id","Light2D")]
    [SerializeField] SerializedDictionary<string, Light2D> lights = new SerializedDictionary<string, Light2D>();
    [SerializeField] SerializedDictionary<string, float> default_intensities = new SerializedDictionary<string, float>();
    Dictionary<string, Coroutine> light_states = new Dictionary<string, Coroutine>();
    void Awake(){
        instance = this;
        create_states();
    }
    public void state_switch(ref Coroutine state, IEnumerator _state){
        if(state != null)
            StopCoroutine(state);
        state = StartCoroutine(_state);
    }
    public void create_states(){
        foreach(string key in lights.Keys)
            light_states.Add(key, null);
    }
    public void create_defaults(){
        foreach(KeyValuePair<string, Light2D> light in lights)
            default_intensities.Add(light.Key, light.Value.intensity);
    }
    public void set_default_intensity(string id, float value) => default_intensities[id] = value;
    public void reset_intensity(string id, float time) => lerp_intensity(id, default_intensities[id], time, null);
    public void lerp_intensity(string id, float value, float time, System.Action callback = null){
        Coroutine state = light_states[id];
        Light2D light = lights[id];
        state_switch(ref state, Calc.lerp_value(
            _val=>light.intensity=_val,
            _start:light.intensity,
            _end:value,
            _time:time,
            _on_complete:()=> callback?.Invoke()));
    }
    public void enable_light(string id, bool enable) => lights[id].enabled = enable;
    public void set_intensity(string id, float value) => lights[id].intensity = value;
}
