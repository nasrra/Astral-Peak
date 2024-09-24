using UnityEngine;

// maybe create a creature or character class that has this component and a status effect component.
// enemies could hurt eachother and player can hurt them.
// interactivity and all that.

public class Health : MonoBehaviour{
    [SerializeField] private int max = 3;
    [SerializeField] private int current = 3;

    public void heal(int amt){
        current += amt;
        if(current > max)
            current = max;
    }

    public void damage(int amt){
        current -= amt;
    }
}