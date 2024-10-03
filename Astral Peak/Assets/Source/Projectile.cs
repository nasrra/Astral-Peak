using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour{
    [SerializeField] private int damage = 1;
    [SerializeField] private Movement movement;

    void Awake(){
        // start moving right
        movement.move_right(true);
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.GetComponent<Creature>() != null)
            // damage creature that is hit.
            other.GetComponent<Creature>().damage(damage, null);
        Destroy(gameObject);
    }
}
