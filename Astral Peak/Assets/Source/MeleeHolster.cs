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
    [SerializeField] private ParticleSystem particles;
    void Start() => link_events();
    
    public void enable_hurt_box(int x) => hurt_box.enabled = x != 0; 
    public void set_damage(int amt) => damage = amt;
    public float get_self_damage() => self_damage;
    public float get_self_knockback_force() => self_knockback_force;
    public float get_self_knockback_duration() => self_knockback_duration;

    public void hit(Collider2D other){
        //Debug.Log(other.gameObject.name + " hit with melee!");
        Creature creature = other.GetComponent<Creature>();
        
        // if we damage the creature.
        if(creature.damage(damage) == true)
            // knock it back.
            creature.get_movement().knockback(other.transform.position - transform.position, knockback_force, knockback_duration);
        // if not.
        else
            // knock us back.
            hit_enemy_guard?.Invoke(other);
    }

    // used for when a creature changes their facing direction.
    public void flip_particle_emitter_left() => particles.GetComponent<ParticleSystemRenderer>().flip = new Vector3(0,0,0);
    public void flip_particle_emitter_right() => particles.GetComponent<ParticleSystemRenderer>().flip = new Vector3(1,0,0);

    public void slash_effect() => particles.Emit(1);

    public void link_events() => feedback.trigger_enter += hit;
    public void unlink_events() =>feedback.trigger_enter -= hit;
}