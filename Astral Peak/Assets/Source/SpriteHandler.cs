using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entropek;
using AYellowpaper.SerializedCollections;
using System;

public class SpriteHandler : MonoBehaviour{
    // time = 0.35f.
    [SerializeField] protected SerializedDictionary<string,SpriteRenderer> sprites;

    public void set_material(Material material){
        foreach(SpriteRenderer sprite in sprites.Values)
            sprite.material = material;
    }

    public void state_switch(ref Coroutine state, IEnumerator _state){
        if(state != null)
            StopCoroutine(state);
        state = StartCoroutine(_state);
    }

    public IEnumerator pulse_value(string _value, float _start, float _end, float _time, int _pulses){
        List<string> renderers = new List<string>();
        foreach(string renderer in sprites.Keys)
            renderers.Add(renderer);
        yield return pulse_value(renderers, _value, _start, _end, _time, _pulses);
    }

    public IEnumerator pulse_value(List<string> _sprite_id, string _value, float _start, float _end, float _time, int _pulses){
        int count = _pulses;
        List<SpriteRenderer> renderers = new List<SpriteRenderer>();
        foreach(string id in _sprite_id)
            renderers.Add(sprites[id]);
        while (count > 0){
            yield return StartCoroutine(lerp_value(renderers, _value, _start, _end, _time));
            --count;
            yield return null;
        }
        foreach (SpriteRenderer s in sprites.Values)
            s.material.SetFloat(_value, _end);
        yield break;
    }

    public IEnumerator pulse_value(string _sprite_id, string _value, float _start, float _end, float _time, int _pulses){
        int count = _pulses;
        while (count > 0){
            yield return StartCoroutine(lerp_value(_sprite_id, _value, _start, _end, _time));
            --count;
            yield return null;
        }
        sprites[_sprite_id].material.SetFloat(_value, _end);
        yield break;
    }

    public IEnumerator lerp_value(string value, float start, float end, float time, Action callback = null){
        List<SpriteRenderer> renderers = new List<SpriteRenderer>();
        foreach(SpriteRenderer renderer in sprites.Values)
            renderers.Add(renderer);
        yield return lerp_value(renderers, value, start, end, time, callback);
    }

    public IEnumerator lerp_value(List<SpriteRenderer> sprites, string value, float start, float end, float time, Action callback = null) {
        List<bool> operations = new List<bool>();
        int index = 0;
        foreach(SpriteRenderer sprite in sprites){
            sprite.material.SetFloat(value, start);
            int _index = index;
            index++;
            operations.Add(true); // operation is occuring
            StartCoroutine(Calc.lerp_value(val => sprite.material.SetFloat(value, val), start, end, time, () => operations[_index]=false));
        }
        while(operations.Contains(true)){
            yield return null;
        }
        callback?.Invoke();
        yield break;
    }

    public IEnumerator lerp_value(string sprite_id, string value, float start, float end, float time, Action callback = null) {
        sprites[sprite_id].material.SetFloat(value, start);
        yield return Calc.lerp_value(val => sprites[sprite_id].material.SetFloat(value, val), start, end, time, () => callback?.Invoke());
    }

    public IEnumerator lerp_color(string value, Color start, Color end, float time) {
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

    public void set_color(string value_id, Color color){
        foreach(string sprite in sprites.Keys)
            set_color(sprite, value_id, color);        
    }

    public void set_color(string sprite_id, string value_id, Color color) => sprites[sprite_id].material.SetColor(value_id, color);

    public void set_value(string value_id, float value){
        foreach(string sprite in sprites.Keys)
            set_value(sprite, value_id, value);
    }
    public void set_value(string sprite_id, string value_id, float value) => sprites[sprite_id].material.SetFloat(value_id, value);

    public void enable_sprite(string id, bool enabled) => sprites[id].enabled = enabled;
    public void enable_sprite(bool enabled){
        foreach(SpriteRenderer sprite in sprites.Values)
            sprite.enabled = enabled;
    }

    public void set_sorting_layer(int layer, int order){
        foreach(SpriteRenderer sprite in sprites.Values){
            sprite.sortingLayerID   = layer;
            sprite.sortingOrder     = order;
        }
    }

    public void set_sorting_layer(string sprite_id, int layer, int order){
        sprites[sprite_id].sortingLayerID   = layer;
        sprites[sprite_id].sortingOrder     = order;
    }
}
