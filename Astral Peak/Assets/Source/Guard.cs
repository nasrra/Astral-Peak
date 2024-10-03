using System;
using System.Collections;
using UnityEditor.Rendering;
using UnityEngine;

public class Guard : MonoBehaviour{
    public event Action
        damaged, broken;

    [SerializeField] private bool
        parrying, guarding;
    public bool guard_broke;

    [SerializeField] private float 
        max_guard, current_guard, 
        guard_decay_rate, guard_decay_buffer,
        guard_break_timer;

    [SerializeField] protected Bar bar;

    private Coroutine coroutine;

    void Start() {
        bar.set_bar_max_value(max_guard);
        bar.set_bar_value(current_guard);
    }

    public bool damage(float amt){
        // note: set parrying to off once this is done.
        if(guarding == true || parrying == true)
            return false;

        set_guard_value(current_guard += amt);

        if(current_guard >= max_guard)
            break_guard();
        else{
            damaged?.Invoke();
            if(coroutine != null)
                StopCoroutine(coroutine);
            coroutine = StartCoroutine(guard_decay());
        } 
        return true;    
    }

    private IEnumerator guard_decay(){
        float buffer_timer = guard_decay_buffer;
        while(buffer_timer >= 0.0f){
            buffer_timer -= Time.deltaTime;
            yield return null;
        }
        while(current_guard > 0.0f){
            set_guard_value(current_guard -= Time.deltaTime * guard_decay_rate);
            yield return null;
        }
        set_guard_value(0.0f);
        yield break;
    }

    public void break_guard(){
        set_guard_value(max_guard);
        if(coroutine != null)
            StopCoroutine(coroutine);
        coroutine = StartCoroutine(break_decay());  
        broken?.Invoke();
    }

    private IEnumerator break_decay(){ 
        guard_broke = true;
        float timer = guard_break_timer;
        while(timer >= 0.0f){
            timer -= Time.deltaTime;
            yield return null;
        }
        guard_broke = false;
        refresh_guard();
        yield break;
    }

    public void refresh_guard(){
        set_guard_value(0.0f);
        //on_guard_refresh?.Invoke();
    }

    private void set_guard_value(float x){
        current_guard = x;
        bar.set_bar_value(current_guard);
    }

    // for animtion key events:
    public void is_guarding(int x) => guarding = x != 0;
    public void is_parrying(int x) => parrying = x != 0;
    
    // for guard bar to update itself.
    public float get_max_guard() => max_guard;
    public float get_current_guard() => current_guard; 
}
