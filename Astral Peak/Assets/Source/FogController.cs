using System.Collections;
using System.Collections.Generic;
using System.IO.Compression;
using UnityEditor.Tilemaps;
using UnityEngine;

public class FogController : SpriteHandler{
    [SerializeField] List<FogShaderData> fog_presets = new List<FogShaderData>();
    Coroutine
        size_state,
        density_state,
        speed_state,
        color_state;
    [SerializeField] float speed = 2;
    void Awake(){
        StartCoroutine(test());
        StartCoroutine(set_offset());   
        set_preset(0);
    }

    public void set_preset(int _preset){
        Material fog = sprites[0].material;
        FogShaderData data = fog_presets[_preset];
        fog.SetFloat("_size", data.size);
        fog.SetFloat("_density",data.density);
        fog.SetColor("_color",data.color);        
        speed = data.speed;
    }

    public void lerp_preset(int _preset){
        Material fog = sprites[0].material;
        FogShaderData data = fog_presets[_preset];
        state_switch(ref size_state,lerp_value      ("_size",fog.GetFloat("_size"), data.size, 1));
        state_switch(ref density_state,lerp_value   ("_density",fog.GetFloat("_density"),data.density,1));
        state_switch(ref speed_state,lerp_speed     (speed,data.speed,2));
        state_switch(ref color_state,lerp_color     ("_color",fog.GetColor("_color"),data.color,1));
    }

    protected IEnumerator lerp_speed(float start, float end, float time) {
        float elapsedTime = 0;
        float t = 0;
        speed = start;
        while (t < time) {
            elapsedTime += Time.deltaTime;
            t = elapsedTime / time;
            speed = Mathf.Lerp(start,end,1/time * t);
            yield return null;
        }
        speed = end;
        yield break;
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
            sprites[0].material.SetVector("_offset", sprites[0].material.GetVector("_offset") + (new Vector4(1, 0,0,0) * speed * Time.deltaTime));
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
