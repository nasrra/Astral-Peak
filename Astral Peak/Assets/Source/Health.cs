using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;

// maybe create a creature or character class that has this component and a status effect component.
// enemies could hurt eachother and player can hurt them.
// interactivity and all that.

public class Health : MonoBehaviour{
    public event Action
        healed, death, damaged;
    
    [SerializeField] private float
        max_life, current_life;

    [SerializeField] protected Bar bar;

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

    // damage is an ambiguos function that handles damaging life values as well as guard.
    public bool damage(float amt){
        current_life -= amt;
        if(current_life <= 0.0f){
            current_life = 0.0f;
            bar.set_bar_value(current_life);
            death?.Invoke();
        }
        bar.set_bar_value(current_life);
        damaged?.Invoke();
        return true;
    }
}