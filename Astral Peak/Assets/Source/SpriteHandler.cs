using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteHandler : MonoBehaviour{
    [SerializeField] List<SpriteRenderer> sprites;
    [SerializeField] Coroutine state;
    [SerializeField] Color damaged_colour;

    protected void switch_state(IEnumerator state){
        if(this.state != null)
            StopCoroutine(this.state);
        this.state = StartCoroutine(state);
    }

    public virtual void play_damaged_flash() => switch_state(damaged_flash());
    protected IEnumerator damaged_flash(int amount = 1, float time = 0.3f) {
        int count = amount;
        float elapsedTime = 0f;
        float hue = 0;
        set_colour(damaged_colour);

        while (count > 0){
            while (elapsedTime < time) {
                elapsedTime += Time.deltaTime;
                hue = Mathf.Lerp(1f,0f,elapsedTime/time);
                foreach(SpriteRenderer s in sprites)
                    s.material.SetFloat("_amount",hue);
                yield return null;
            }
            elapsedTime = 0;
            --count;
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
