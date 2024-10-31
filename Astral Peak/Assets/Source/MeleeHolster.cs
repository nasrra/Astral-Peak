using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEditor;

//create seperate classes for each atttack

[Serializable]
public class MeleeHolster{
    public delegate void AnimationDelegate();
    private AnimationDelegate animation;

    public Action<Collider2D>
        hit_enemy_guard;
    [SerializeField]private float 
        damage, knockback_force, knockback_duration,
        self_knockback_force, self_knockback_duration;
    
    [SerializeField] protected Collider2D hurt_box;
    [SerializeField] protected Collider2DFeedback feedback;
    [SerializeField] protected List<ParticleSystem> particles;
    protected Transform transform;

    public void set_animation(AnimationDelegate animation) => this.animation = animation;
    public void set_transform(Transform transform) => this.transform = transform; 

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
    public void flip_particle_emitter_left(){
        foreach(ParticleSystem p in particles)
            p.GetComponent<ParticleSystemRenderer>().flip = new Vector3(0,0,0);
    }
    public void flip_particle_emitter_right(){
        foreach(ParticleSystem p in particles)
            p.GetComponent<ParticleSystemRenderer>().flip = new Vector3(1,0,0);
    }

    public void emit_particles(){
        foreach(ParticleSystem p in particles){
            p.Emit(1);

            // flip the orientation of the slash particle for variety.
            Vector3 current_flip = p.GetComponent<ParticleSystemRenderer>().flip; 
            //current_flip.y = UnityEngine.Random.Range(0,2);
            p.GetComponent<ParticleSystemRenderer>().flip = current_flip;
        }
    }

    public void use() => animation();
}