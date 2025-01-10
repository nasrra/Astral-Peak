using System;
using System.Collections;
using System.Security.Cryptography;
using Deluz;
using UnityEngine;

// maybe create a creature or character class that has this component and a status effect component.
// enemies could hurt eachother and player can hurt them.
// interactivity and all that.

public class Health : MonoBehaviour{
    public event Action
        healed, death, damaged, now_invulnerable, now_vulnerable;
    public event Action<KnockbackData> 
        knockback;
    [SerializeField] protected HealthData data;
    [SerializeField] public bool invulnerable;
    Coroutine invulnerable_state;

    public ref int get_current_health() => ref data.current_life;
    public ref int get_max_health() => ref data.max_life;

    public void state_switch(ref Coroutine state, IEnumerator coroutine){
        if(state!=null)
            StopCoroutine(state);
        state = StartCoroutine(coroutine);
    }

    public void set_data(HealthData _data) => data = _data;

    public void heal(int amt){
        data.current_life += amt;
        if(data.current_life > data.max_life)
            data.current_life = data.max_life;
        else
            healed?.Invoke();
    }

    public void is_invulnerable(){
        invulnerable = true;
        now_invulnerable?.Invoke();
    }
    public void is_invulnerable(float time) => 
        state_switch(ref invulnerable_state, Util.timer(time,
            start_action: ()=>is_invulnerable(),
            time_out: ()=>{
                invulnerable_state=null;
                is_vulnerable();
            }));

    public void is_vulnerable(){
        if(invulnerable_state == null){
            invulnerable = false;
            now_vulnerable?.Invoke();
        }
    }

    public void set_max_life(int amount) => data.max_life = amount;
    public void set_current_life(int amount) => data.current_life = amount;

    // damage is an ambiguos function that handles damaging life values as well as guard.
    public void damage(DamageData damage_data, KnockbackData knockback_data){
        if(invulnerable == true)
            return;
        
        // deal damage.
        data.current_life -= damage_data.damage;
        Action action = data.current_life <= 0? death : damaged;
        action?.Invoke();

        // invoke knockback if needed.
        if(knockback_data != null)
            knockback?.Invoke(knockback_data);
    }
}

[Serializable]
public struct HealthData{
    public int max_life, current_life;
    public HealthData(int _max_life, int _current_life){
        max_life = _max_life;
        current_life = _current_life;
    }
}