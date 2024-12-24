using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteHandler : MonoBehaviour{
    [SerializeField] protected List<SpriteRenderer> sprites;

    protected void set_material(Material material){
        foreach(SpriteRenderer sprite in sprites)
            sprite.material = material;
    }

    protected void state_switch(ref Coroutine state, IEnumerator _state){
        if(state != null)
            StopCoroutine(state);
        state = StartCoroutine(_state);
    }

    protected IEnumerator none(){
        yield break;
    }

    protected IEnumerator pulse_value(string value, int pulses = 1, float time = 0.35f){
        int count = pulses;
        float elapsedTime = 0;
        float t = 0;

        while (count > 0){
            while (t < time) {
                elapsedTime += Time.deltaTime;
                t = elapsedTime / time;
                foreach (SpriteRenderer s in sprites)
                    s.material.SetFloat(value, Mathf.Lerp(1f,0f,1/time * t));
                yield return null;
            }
            elapsedTime = 0;
            t = 0;
            --count;
            yield return null;
        }
        foreach(SpriteRenderer s in sprites)
            s.material.SetFloat(value,0);
        yield break;
    }

    protected IEnumerator lerp_value(string value, float start, float end, float time) {
        float elapsedTime = 0;
        float t = 0;
        foreach (SpriteRenderer s in sprites)
            s.material.SetFloat(value, start);
        while (t < time) {
            elapsedTime += Time.deltaTime;
            t = elapsedTime / time;
            foreach (SpriteRenderer s in sprites)
                s.material.SetFloat(value, Mathf.Lerp(start,end,1/time * t));
            yield return null;
        }
        foreach (SpriteRenderer s in sprites)
            s.material.SetFloat(value, end);
        yield break;
    }

    protected IEnumerator lerp_color(string value, Color start, Color end, float time) {
        float elapsedTime = 0;
        float t = 0;
        foreach (SpriteRenderer s in sprites)
            s.material.SetColor(value, start);
        while (t < time) {
            elapsedTime += Time.deltaTime;
            t = elapsedTime / time;
            foreach (SpriteRenderer s in sprites)
                s.material.SetColor(value, Color.Lerp(start,end,1/time * t));
            yield return null;
        }
        foreach (SpriteRenderer s in sprites)
            s.material.SetColor(value, end);
        yield break;
    }
}
