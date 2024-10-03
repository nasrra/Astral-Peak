using System;
using UnityEngine;

public class MeleeHolster : MonoBehaviour{
    public Action<Collider2D>
        hit_enemy_guard;

    [SerializeField] private float 
        damage, knockback_force, knockback_duration,
        self_damage, self_knockback_force, self_knockback_duration;
    [SerializeField] private Collider2D hurt_box;
    [SerializeField] private Collider2DFeedback feedback;
    void Start() => link_events();
    
    public void enable_hurt_box(int x) => hurt_box.enabled = x != 0; 
    public void set_damage(int amt) => damage = amt;
    public float get_self_damage() => self_damage;
    public float get_self_knockback_force() => self_knockback_force;
    public float get_self_knockback_duration() => self_knockback_duration;

    public void hit(Collider2D other){
        Debug.Log(other.gameObject.name + " hit with melee!");
        Creature creature = other.GetComponent<Creature>();
        if(creature.damage(damage, gameObject) == true)
            creature.get_movement().knockback(other.transform.position - transform.position, knockback_force, knockback_duration);
        else
            hit_enemy_guard?.Invoke(other);
    }

    public void link_events() => feedback.trigger_enter += hit;
    public void unlink_events() =>feedback.trigger_enter -= hit;

}