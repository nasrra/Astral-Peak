using System;
using Entropek;
using UnityEngine;

public class Health : MonoBehaviour{
    public event Action
        healed, death, damaged, now_invulnerable, now_vulnerable, now_guarded;
    public event Action<KnockbackData> 
        knockback;
    [SerializeField] protected HealthData data;
    [SerializeField] HealthState state;

    public ref int get_current_health() => ref data.current_life;
    public ref int get_max_health() => ref data.max_life;

    public void set_data(HealthData _data) => data = _data;

    public void heal(int amt){
        data.current_life += amt;
        if(data.current_life > data.max_life)
            data.current_life = data.max_life;
        else
            healed?.Invoke();
    }

    public void set_guarded(){
        state = HealthState.GUARDED;
        now_guarded?.Invoke();
    }

    public void set_vulnerable(){
        state = HealthState.VULNERABLE;
        now_vulnerable?.Invoke();
    }

    public void set_invulnerable(){
        state = HealthState.INVULNERABLE;
        now_invulnerable?.Invoke();
    }

    public void set_max_life(int amount) => data.max_life = amount;
    public void set_current_life(int amount) => data.current_life = amount;

    // damage is an ambiguos function that handles damaging life values as well as guard.
    public void damage(DamageData damage_data, KnockbackData knockback_data){
        if(state == HealthState.INVULNERABLE)
            return;
        if(damage_data.unblockable == true || state == HealthState.VULNERABLE){
            damage(damage_data.damage);
            if(knockback_data != null)
                knockback?.Invoke(knockback_data);
        }
        // invoke knockback if needed.
    }

    private void damage(int amount){
        // deal damage.
        data.current_life -= amount;
        if(data.current_life <= 0)
            death?.Invoke();
        else
            damaged?.Invoke();
    }

    enum HealthState : sbyte{
        VULNERABLE,
        GUARDED,
        INVULNERABLE,
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