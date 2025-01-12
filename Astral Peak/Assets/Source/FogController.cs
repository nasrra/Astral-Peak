using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entropek;

public class FogController : SpriteHandler{
    [SerializeField] List<FogShaderData> fog_presets = new List<FogShaderData>();
    Coroutine
        size_state,
        density_state,
        speed_state,
        color_state;
    [SerializeField] float speed = 2;
    void Awake(){
        //StartCoroutine(test());
        StartCoroutine(set_offset());   
        set_preset(0);
    }

    public void set_preset(int _preset){
        Material fog = sprites["main"].material;
        FogShaderData data = fog_presets[_preset];
        fog.SetFloat("_size", data.size);
        fog.SetFloat("_density",data.density);
        fog.SetColor("_color",data.color);        
        speed = data.speed;
    }

    public void lerp_preset(int _preset){
        Material fog = sprites["main"].material;
        FogShaderData data = fog_presets[_preset];
        state_switch(ref size_state,lerp_value      ("_size",fog.GetFloat("_size"), data.size, 2));
        state_switch(ref density_state,lerp_value   ("_density",fog.GetFloat("_density"),data.density,2));
        state_switch(ref speed_state, Calc.lerp_value(val => speed = val, speed, data.speed,2));
        state_switch(ref color_state,lerp_color     ("_color",fog.GetColor("_color"),data.color,2));
    }

    IEnumerator test(){
        while(true){
            yield return new WaitForSeconds(7);
            lerp_preset(1);
            yield return new WaitForSeconds(7);
            lerp_preset(0);
            yield return null;        
        }
    }

    IEnumerator set_offset(){
        while(true){
            sprites["main"].material.SetVector("_offset", sprites["main"].material.GetVector("_offset") + (new Vector4(1, 0,0,0) * speed * Time.deltaTime));
            yield return null;
        }
    }
}

[System.Serializable]
struct FogShaderData{
    public float size;
    public float density;
    public float speed;
    public Color color;
    public FogShaderData(float _size, float _density, float _speed, Color _color){
        size = _size;
        density = _density;
        speed = _speed;
        color = _color;
    }
}
