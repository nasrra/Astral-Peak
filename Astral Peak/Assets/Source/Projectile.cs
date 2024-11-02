using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour{
    [SerializeField] private int damage = 1;
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] protected Transform front_point;
    [SerializeField] protected float speed = 25;
    [SerializeField] TrailRenderer trail;
    [SerializeField] Collider2D col;

    void Awake() => move();

    protected virtual void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.layer == LayersManager.PLAYER){
            Creature creature = other.GetComponent<Creature>();
            if(creature != null&& creature.damage(damage) == true){
                other.GetComponent<Movement>().knockback(other.transform.position - transform.position, 20, 0.25f);
                Destroy(gameObject);
                enable_trail(false);            
            }
        }
        else if(other.gameObject.layer == LayersManager.GROUND){
                Destroy(gameObject);
                enable_trail(false);
        }
    }

    protected virtual void move() => rb.velocity = (front_point.position - transform.position).normalized * speed;
    protected virtual void stop() => rb.velocity = Vector2.zero;
    public void enable_trail(bool x) {if(trail != null)trail.enabled = x;}
    public void enable_collider(bool x) {if(col != null)col.enabled = x;}
}
