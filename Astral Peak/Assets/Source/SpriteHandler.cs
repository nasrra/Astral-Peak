using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteHandler : MonoBehaviour{
    [SerializeField] List<SpriteRenderer> sprites;
    [SerializeField] Coroutine state;
    [SerializeField] Color damaged_colour;

    protected void set_material(Material material){
        foreach(SpriteRenderer sprite in sprites)
            sprite.material = material;
    }

    protected void state_switch(IEnumerator _state){
        if(state != null)
            StopCoroutine(state);
        state = StartCoroutine(_state);
        Debug.Log("switch");
    }

    protected IEnumerator none(){
        yield break;
    }

    protected IEnumerator pulse_value(string value, int pulses = 1, float time = 0.35f){
        int count = pulses;
        float elapsedTime = 0;
        float t = 0;

        while (count > 0){
            while (elapsedTime < t) {
                elapsedTime += Time.deltaTime;
                t = elapsedTime / time;
                foreach (SpriteRenderer s in sprites)
                    s.material.SetFloat(value, Mathf.Lerp(1f,0f,1/time * t));
                yield return null;
            }
            elapsedTime = 0;
            --count;
            yield return null;
        }
        foreach(SpriteRenderer s in sprites)
            s.material.SetFloat(value,1);
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

    public void set_colour(Color color){
        foreach(SpriteRenderer s in sprites)
            s.material.SetColor("_colour", damaged_colour);
    }

    //void OnDestroy() => StopAllCoroutines();
}
