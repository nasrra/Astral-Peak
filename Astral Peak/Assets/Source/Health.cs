using System;
using System.Collections;
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
    [SerializeField] protected int
        max_life, current_life;
    [SerializeField] public bool invulnerable;
    Coroutine invulnerable_state;

    public int get_current_health() => current_life;
    public int get_max_health() => max_life;

    public void state_switch(ref Coroutine state, IEnumerator coroutine){
        if(state!=null)
            StopCoroutine(state);
        state = StartCoroutine(coroutine);
    }

    public void heal(int amt){
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

    public void set_max_life(int amount) => max_life = amount;
    public void set_current_life(int amount) => current_life = amount;

    // damage is an ambiguos function that handles damaging life values as well as guard.
    public void damage(DamageData damage_data, KnockbackData knockback_data){
        if(invulnerable == true)
            return;
        
        // deal damage.
        current_life -= damage_data.damage;
        Action action = current_life <= 0? death : damaged;
        action?.Invoke();

        // invoke knockback if needed.
        if(knockback_data != null)
            knockback?.Invoke(knockback_data);
    }
}