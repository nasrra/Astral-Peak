using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entropek;
using AYellowpaper.SerializedCollections;
using System;
using UnityEngine.UI;

public class ImageHandler : MonoBehaviour{
    [SerializeField] protected SerializedDictionary<string,Image> images;

    protected void set_material(Material material){
        foreach(Image image in images.Values)
            image.material = material;
    }

    protected void state_switch(ref Coroutine state, IEnumerator _state){
        if(state != null)
            StopCoroutine(state);
        state = StartCoroutine(_state);
    }

    protected IEnumerator lerp_renderer_color(string image_id, Color start, Color end, float time){
        yield return Calc.lerp_color(color=>images[image_id].color=color, start, end, time);
    }

    protected IEnumerator pulse_material_value(string value, int pulses = 1, float time = 0.35f){
        int count = pulses;
        while (count > 0){
            yield return StartCoroutine(lerp_material_value(value, 1f, 0f, time));
            --count;
            yield return null;
        }
        foreach (Image i in images.Values)
            i.material.SetFloat(value, 0);
        yield break;
    }

    protected IEnumerator pulse_material_value(string image_id, string value, int pulses = 1, float time = 0.35f){
        int count = pulses;
        while (count > 0){
            yield return StartCoroutine(lerp_material_value(image_id, value, 1f, 0f, time));
            --count;
            yield return null;
        }
        images[image_id].material.SetFloat(value, 0);
        yield break;
    }

    protected IEnumerator lerp_material_value(string value, float start, float end, float time, Action callback = null) {
        List<bool> operations = new List<bool>();
        int index = 0;
        foreach(KeyValuePair<string, Image> kvp in images){
            images[kvp.Key].material.SetFloat(value, start);
            int _index = index;
            index++;
            operations.Add(true); // operation is occuring
            StartCoroutine(Calc.lerp_value(val => images[kvp.Key].material.SetFloat(value, val), start, end, time, () => operations[_index]=false));
        }
        while(operations.Contains(true)){
            yield return null;
        }
        callback?.Invoke();
        yield break;
    }

    protected IEnumerator lerp_material_value(string image_id, string value, float start, float end, float time, Action callback = null) {
        images[image_id].material.SetFloat(value, start);
        yield return Calc.lerp_value(val => images[image_id].material.SetFloat(value, val), start, end, time, () => callback?.Invoke());
    }

    protected IEnumerator lerp_material_color(string value, Color start, Color end, float time) {
        List<bool> operations = new List<bool>();
        int index = 0;
        foreach(KeyValuePair<string, Image> kvp in images){
            images[kvp.Key].material.SetColor(value, start);
            int _index = index;
            index++;
            operations.Add(true); // operation is occuring
            StartCoroutine(Calc.lerp_color(val => images[kvp.Key].material.SetColor(value, val), start, end, time, () => operations[_index]=false));
        }
        while(operations.Contains(true)){
            yield return null;
        }
        yield break;
    }

    protected void set_material_value(string value_id, float value){
        foreach(string image in images.Keys)
            set_material_value(image, value_id, value);
    }
    protected void set_material_value(string sprite_id, string value_id, float value) => images[sprite_id].material.SetFloat(value_id, value);

    public void enable_material_image(string id, bool enabled) => images[id].enabled = enabled;
    public void enable_material_image(bool enabled){
        foreach(Image image in images.Values)
            image.enabled = enabled;
    }
}
