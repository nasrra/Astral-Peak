using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class SpriteHandler : MonoBehaviour{
    [SerializeField] List<SpriteRenderer> sprites;
    [SerializeField] Coroutine state;
    [SerializeField] Color damaged_colour;

    protected void set_material(Material material){
        foreach(SpriteRenderer sprite in sprites)
            sprite.material = material;
    }

    protected void state_switch(IEnumerator state){
        if(this.state != null)
            StopCoroutine(this.state);
        this.state = StartCoroutine(state);
    }

    protected IEnumerator pulse_value(string value, int pulses = 1, float time = 0.35f){
        int count = pulses;
        float elapsedTime = 0f;

        while (count > 0){
            while (elapsedTime < time) {
                elapsedTime += Time.deltaTime;
                float amount = Mathf.Lerp(1f,0f,elapsedTime/time);
                foreach(SpriteRenderer s in sprites)
                    s.material.SetFloat(value,amount);
                yield return null;
            }
            elapsedTime = 0;
            --count;
            yield return null;
        }
        yield break;
    }

    protected IEnumerator lerp_value(string value, float start, float end, float time){
        float elapsedTime = 0;
        while (elapsedTime < time) {
            elapsedTime += Time.deltaTime;
            float amount = Mathf.Lerp(start,end,elapsedTime/time);
            foreach(SpriteRenderer s in sprites)
                s.material.SetFloat(value, amount);
            yield return null;
        }
        yield break;
    }

    public void set_colour(Color color){
        foreach(SpriteRenderer s in sprites)
            s.material.SetColor("_colour", damaged_colour);
    }

    //void OnDestroy() => StopAllCoroutines();
}
