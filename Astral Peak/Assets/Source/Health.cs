using System;
using UnityEngine;

// maybe create a creature or character class that has this component and a status effect component.
// enemies could hurt eachother and player can hurt them.
// interactivity and all that.

public class Health : MonoBehaviour{
    public event Action 
        on_death, on_heal, on_damage;

    [SerializeField] private bool invulnerable = false;
    [SerializeField] private int max = 3;
    [SerializeField] private int current = 3;

    public void heal(int amt){
        current += amt;
        if(current > max)
            current = max;
        else
            on_heal?.Invoke();
    }

    public void damage(int amt){
        current -= amt;
        on_damage?.Invoke();
        if(current <= 0)
            on_death?.Invoke();
    }

    public void set_invulnerable(int x){
        //Debug.Log(gameObject.name + " is now invulnerable.");
        invulnerable = x != 0;
    }
}