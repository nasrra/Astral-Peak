using System;
using UnityEngine;

//create seperate classes for each atttack

[Serializable]
public class MeleeHolster{
    public delegate void AnimationDelegate();
    protected AnimationDelegate animation;

    public Action<Collider2D>
        hit_enemy_guard;

    [SerializeField] protected Transform transform;    
    [SerializeField] protected Collider2D hurt_box;
    [SerializeField] protected Collider2DFeedback feedback;

    [SerializeField]private float 
        damage, knockback_force, knockback_duration,
        self_knockback_force, self_knockback_duration;

    public void set_animation(AnimationDelegate animation) => this.animation = animation;

    public void enable_hurt_box(int x){
        if(x != 0){
            feedback.trigger_enter += hit;
            hurt_box.enabled = true;
        }
        else{
            feedback.trigger_enter -= hit;
            hurt_box.enabled = false;
        }
    }
    public void set_damage(int amt) => damage = amt;
    public float get_self_knockback_force() => self_knockback_force;
    public float get_self_knockback_duration() => self_knockback_duration;

    public void hit(Collider2D other){
        Creature direct = other.GetComponent<Creature>(); // direct creature reference from smaller enemies with one collider; like human characters.
        CreatureLink link = other.GetComponent<CreatureLink>(); // creature linker for bigger creatures with multiple colliders and segments.

        Creature creature = (direct != null)? direct : link.get_creature();

        // if we damage the creature.
        if(creature.damage(damage) == true)
            // knock it back.
            creature.get_movement().knockback(other.transform.position - transform.position, knockback_force, knockback_duration);
        // if not.
        else
            // knock us back.
            hit_enemy_guard?.Invoke(other);
    }

    public virtual void use() => animation();
}