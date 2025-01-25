using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entropek;
using AYellowpaper.SerializedCollections;
using System;

public class SpriteHandler : MonoBehaviour{
    [SerializeField] protected SerializedDictionary<string,SpriteRenderer> sprites;

    protected void set_material(Material material){
        foreach(SpriteRenderer sprite in sprites.Values)
            sprite.material = material;
    }

    protected void state_switch(ref Coroutine state, IEnumerator _state){
        if(state != null)
            StopCoroutine(state);
        state = StartCoroutine(_state);
    }

    protected IEnumerator pulse_value(string value, int pulses = 1, float time = 0.35f){
        int count = pulses;
        while (count > 0){
            yield return StartCoroutine(lerp_value(value, 1f, 0f, time));
            --count;
            yield return null;
        }
        foreach (SpriteRenderer s in sprites.Values)
            s.material.SetFloat(value, 0);
        yield break;
    }

    protected IEnumerator pulse_value(string sprite_id, string value, int pulses = 1, float time = 0.35f){
        int count = pulses;
        while (count > 0){
            yield return StartCoroutine(lerp_value(sprite_id, value, 1f, 0f, time));
            --count;
            yield return null;
        }
        sprites[sprite_id].material.SetFloat(value, 0);
        yield break;
    }

    protected IEnumerator lerp_value(string value, float start, float end, float time, Action callback = null) {
        List<bool> operations = new List<bool>();
        int index = 0;
        foreach(KeyValuePair<string, SpriteRenderer> kvp in sprites){
            sprites[kvp.Key].material.SetFloat(value, start);
            int _index = index;
            index++;
            operations.Add(true); // operation is occuring
            StartCoroutine(Calc.lerp_value(val => sprites[kvp.Key].material.SetFloat(value, val), start, end, time, () => operations[_index]=false));
        }
        while(operations.Contains(true)){
            yield return null;
        }
        callback?.Invoke();
        yield break;
    }

    protected IEnumerator lerp_value(string sprite_id, string value, float start, float end, float time, Action callback = null) {
        sprites[sprite_id].material.SetFloat(value, start);
        yield return Calc.lerp_value(val => sprites[sprite_id].material.SetFloat(value, val), start, end, time, () => callback?.Invoke());
    }

    protected IEnumerator lerp_color(string value, Color start, Color end, float time) {
        List<bool> operations = new List<bool>();
        int index = 0;
        foreach(KeyValuePair<string, SpriteRenderer> kvp in sprites){
            sprites[kvp.Key].material.SetColor(value, start);
            int _index = index;
            index++;
            operations.Add(true); // operation is occuring
            StartCoroutine(Calc.lerp_color(val => sprites[kvp.Key].material.SetColor(value, val), start, end, time, () => operations[_index]=false));
        }
        while(operations.Contains(true)){
            yield return null;
        }
        yield break;
    }

    protected void set_value(string value_id, float value){
        foreach(string sprite in sprites.Keys)
            set_value(sprite, value_id, value);
    }
    protected void set_value(string sprite_id, string value_id, float value) => sprites[sprite_id].material.SetFloat(value_id, value);

    public void enable_sprite(string id, bool enabled) => sprites[id].enabled = enabled;
    public void enable_sprite(bool enabled){
        foreach(SpriteRenderer sprite in sprites.Values)
            sprite.enabled = enabled;
    }
}
