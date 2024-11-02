using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// maybe create a creature or character class that has this component and a status effect component.
// enemies could hurt eachother and player can hurt them.
// interactivity and all that.

public class Health : MonoBehaviour{
    [SerializeField] List<SpriteRenderer> sprite;
    public event Action
        healed, death, damaged;
    [SerializeField] private float
        max_life, current_life;
    [SerializeField] bool invulnerable;
    [SerializeField] protected Bar bar;
    Coroutine flash_coroutine;

    void Start(){
        bar.set_bar_max_value(max_life);
        bar.set_bar_value(current_life);
    }

    public void heal(int amt, GameObject other = null){
        current_life += amt;
        if(current_life > max_life)
            current_life = max_life;
        else
            healed?.Invoke();
    }

    public void is_invulnerable() => invulnerable = true;
    public void is_vulnerable() => invulnerable = false;

    // damage is an ambiguos function that handles damaging life values as well as guard.
    public bool damage(float amt){
        if(invulnerable == true)
            return false;

        current_life -= amt;
        if(current_life <= 0.0f){
            current_life = 0.0f;
            bar.set_bar_value(current_life);
            death?.Invoke();
        }
        switch_flash_coroutine(damage_flash());
        bar.set_bar_value(current_life);
        damaged?.Invoke();
        return true;
    }

    void switch_flash_coroutine(IEnumerator x){
        if(flash_coroutine != null)
            StopCoroutine(flash_coroutine);
        flash_coroutine = StartCoroutine(x);
    }
    
    IEnumerator damage_flash() {
        float elapsedTime = 0f;
        float amount = 0;
        float flash_time = 0.3f;

        while (elapsedTime < flash_time) {
            elapsedTime += Time.deltaTime;
            amount = Mathf.Lerp(1f,0f,elapsedTime/flash_time);
            foreach(SpriteRenderer s in sprite)
                s.material.SetFloat("_amount",amount);
            yield return null;
        }
        yield break;
    }
}