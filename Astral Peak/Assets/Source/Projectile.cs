using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour{
    [SerializeField] private int damage = 1;
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] private Transform front_point;
    [SerializeField] float speed = 25;

    void Awake(){
        // start moving right
        rb.velocity = (front_point.position - transform.position).normalized * speed;
    }

    protected virtual void OnTriggerEnter2D(Collider2D other){
        if(other.GetComponent<Creature>() != null)
            // damage creature that is hit.
            other.GetComponent<Creature>().damage(damage);
        Destroy(gameObject);
    }
}
