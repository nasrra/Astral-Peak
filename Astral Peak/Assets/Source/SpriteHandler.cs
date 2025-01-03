using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Deluz;
using DocumentFormat.OpenXml.Drawing.Charts;

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
        while (count > 0){
            yield return StartCoroutine(lerp_value(value, 1f, 0f, time));
            --count;
            yield return null;
        }
        foreach (SpriteRenderer s in sprites)
            s.material.SetFloat(value, 0);
        yield break;
    }

    protected IEnumerator lerp_value(string value, float start, float end, float time) {
        List<bool> operations = new List<bool>();
        for(int i = 0; i<sprites.Count;++i){
            int index = i;
            sprites[i].material.SetFloat(value, start);
            operations.Add(true); // operation is occuring
            StartCoroutine(Calc.lerp_value(val => sprites[index].material.SetFloat(value, val), start, end, time, () => operations[index]=false));
        }
        while(operations.Contains(true)){
            yield return null;
        }
        yield break;
    }

    protected IEnumerator lerp_color(string value, Color start, Color end, float time) {
        List<bool> operations = new List<bool>();
        for(int i = 0; i<sprites.Count;++i){
            int index = i;
            sprites[i].material.SetColor(value, start);
            operations.Add(true); // operation is occuring
            StartCoroutine(Calc.lerp_color(val => sprites[index].material.SetColor(value, val), start, end, time, () => operations[index]=false));
        }
        while(operations.Contains(true))
            yield return null;
        yield break;
    }

    //protected IEnumerator lerp_color(string value, Color start, Color end, float time) {
    //    float elapsedTime = 0;
    //    float t = 0;
    //    foreach (SpriteRenderer s in sprites)
    //        s.material.SetColor(value, start);
    //    while (t < time) {
    //        elapsedTime += Time.deltaTime;
    //        t = elapsedTime / time;
    //        foreach (SpriteRenderer s in sprites)
    //            s.material.SetColor(value, Color.Lerp(start,end,1/time * t));
    //        yield return null;
    //    }
    //    foreach (SpriteRenderer s in sprites)
    //        s.material.SetColor(value, end);
    //    yield break;
    //}
}
