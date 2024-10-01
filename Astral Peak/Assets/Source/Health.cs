using System;
using UnityEngine;

// maybe create a creature or character class that has this component and a status effect component.
// enemies could hurt eachother and player can hurt them.
// interactivity and all that.

public class Health : MonoBehaviour{
    public event Action<GameObject> 
        on_death, on_heal, on_damage, on_invulnerable;

    [SerializeField] private bool invulnerable = false;
    [SerializeField] private int max = 3;
    [SerializeField] private int current = 3;

    public void heal(int amt, GameObject other = null){
        current += amt;
        if(current > max)
            current = max;
        else
            on_heal?.Invoke(other);
    }

    public void damage(int amt, GameObject other = null){
        if(invulnerable == true){
            on_invulnerable?.Invoke(other);
            return;
        }
        current -= amt;
        on_damage?.Invoke(other);
        if(current <= 0)
            on_death?.Invoke(other);
    }

    public void set_invulnerable(int x){
        //Debug.Log(gameObject.name + " is now invulnerable.");
        invulnerable = x != 0;
    }
}