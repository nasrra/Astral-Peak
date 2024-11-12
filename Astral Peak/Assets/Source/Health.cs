using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Video;

// maybe create a creature or character class that has this component and a status effect component.
// enemies could hurt eachother and player can hurt them.
// interactivity and all that.

public class Health : MonoBehaviour{
    public event Action
        healed, death, damaged, now_invulnerable, now_vulnerable;
    [SerializeField] private int
        max_life, current_life;
    [SerializeField] public bool invulnerable;

    public int get_current_health() => current_life;

    public void heal(int amt, GameObject other = null){
        current_life += amt;
        if(current_life > max_life)
            current_life = max_life;
        else
            healed?.Invoke();
    }

    public void is_invulnerable(){
        invulnerable = true;
        now_invulnerable?.Invoke();
    }
    public void is_invulnerable(float time){
        StopAllCoroutines();
        StartCoroutine(invulnerable_timed(time));
    }

    IEnumerator invulnerable_timed(float time){
        is_invulnerable();
        yield return new WaitForSeconds(time);
        is_vulnerable();
    } 

    public void is_vulnerable(){
        invulnerable = false;
        now_vulnerable?.Invoke();
    }

    // damage is an ambiguos function that handles damaging life values as well as guard.
    public bool damage(int amt){
        if(invulnerable == true)
            return false;

        current_life -= amt;
        if(current_life <= 0){
            current_life = 0;
            death?.Invoke();
        }
        damaged?.Invoke();
        return true;
    }
}