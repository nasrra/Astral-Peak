using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;

// maybe create a creature or character class that has this component and a status effect component.
// enemies could hurt eachother and player can hurt them.
// interactivity and all that.

public class Health : MonoBehaviour{
    private Coroutine coroutine;

    public event Action 
        on_death, on_heal, on_damage_life, on_damage_guard;
    
    public event Action<float>
        on_guard_update;

    public event Action
        on_guard_broken, on_guard_refresh;

    [SerializeField] private bool
        invulnerable, parrying, guarding, guard_broken;
    [SerializeField] private int
        max_life, current_life;
    [SerializeField] private float 
        max_guard, current_guard, 
        guard_decay_rate, guard_decay_buffer,
        guard_break_timer;

    public void heal(int amt, GameObject other = null){
        current_life += amt;
        if(current_life > max_life)
            current_life = max_life;
        else
            on_heal?.Invoke();
    }

    // damage is an ambiguos function that handles damaging life values as well as guard.
    public bool damage(int amt){
        if(guard_broken == true){
            damage_life(amt);
            return true;
        }
        else if (guarding == true){
            return false;
        }
        damage_guard(amt);
        return true;
    }

    private void damage_life(int amt){
        current_life -= amt;
        on_damage_life?.Invoke();
        if(current_life <= 0)
            on_death?.Invoke();
    }

    private void damage_guard(float amount){
        current_guard += amount;
        if(current_guard >= max_guard)
            break_guard();
        else{
            if(coroutine != null)
                StopCoroutine(coroutine);
            coroutine = StartCoroutine(guard_decay());
        }
        on_damage_guard?.Invoke();
        on_guard_update?.Invoke(current_guard);
    }

    private IEnumerator guard_decay(){
        float buffer_timer = guard_decay_buffer;
        while(buffer_timer >= 0.0f){
            buffer_timer -= Time.deltaTime;
            yield return null;
        }
        while(current_guard > 0.0f){
            current_guard -= Time.deltaTime * guard_decay_rate;
            on_guard_update?.Invoke(current_guard);
            yield return null;
        }
        current_guard = 0.0f;
        on_guard_update?.Invoke(current_guard);
        yield break;
    }

    public void break_guard(){
        current_guard = max_guard;
        if(coroutine != null)
            StopCoroutine(coroutine);
        coroutine = StartCoroutine(break_decay());  
        on_guard_broken?.Invoke();
    }

    public void refresh_guard(){
        current_guard = 0.0f;
        on_guard_refresh?.Invoke();
    }

    private IEnumerator break_decay(){ 
        guard_broken = true;
        float timer = guard_break_timer;
        while(timer >= 0.0f){
            timer -= Time.deltaTime;
            yield return null;
        }
        guard_broken = false;
        refresh_guard();
        yield break;
    }

    public void is_invulnerable(int x) => invulnerable = x != 0;
    public void is_guarding(int x) => guarding = x != 0;
    public void is_parrying(int x) => parrying = x != 0;
    public float get_max_guard() => max_guard;
    public float get_current_guard() => current_guard; 
}