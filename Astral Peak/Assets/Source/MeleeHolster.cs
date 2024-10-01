using UnityEngine;

public class MeleeHolster : MonoBehaviour{
    [SerializeField] private int damage = 1;
    [SerializeField] private float knockback_force, knockback_duration;
    [SerializeField] private Collider2D hurt_box;
    [SerializeField] private Collider2DFeedback feedback;
    void Start(){
        link_events();
    }
    public void enable_hurt_box(int x){
        hurt_box.enabled = x != 0; // enable if x is zero and disable otherwise.
    }
    public void set_damage(int amt){
        damage = amt;
    }

    public void hit(Collider2D other){
        Debug.Log(other.gameObject.name + " hit with melee!");
        Creature creature = other.GetComponent<Creature>();
        creature.get_health().damage(damage); // deal damage.
        creature.get_movement().knockback(other.transform.position - transform.position, knockback_force, knockback_duration);
    }

    public void link_events(){
        feedback.trigger_enter += hit;
    }

    public void unlink_events(){
        feedback.trigger_enter -= hit;
    }
}