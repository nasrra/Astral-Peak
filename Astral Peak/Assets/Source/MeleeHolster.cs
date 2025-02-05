using System;
using System.Collections.Generic;
using UnityEngine;

//create seperate classes for each atttack

[Serializable]
public class MeleeHolster{
    public event Action hit_creature;
    public event Action<KnockbackData> self_knockback;
    Dictionary<Creature, bool> hit_creatures = new Dictionary<Creature, bool>();

    [SerializeField] protected Collider2D hurt_box;
    [SerializeField] protected Collider2DFeedback feedback;

    [SerializeField] DamageData damage_data = new DamageData(0);
    [SerializeField] KnockbackData knockback_data = new KnockbackData(0,0,null);

    [SerializeField]private float 
        self_knockback_force, self_knockback_duration;

    // unlink
    void OnDestroy(){
        feedback.trigger_enter -= hit;
    }

    public void enable_hurt_box(bool x){
        if(x == true){
            feedback.trigger_enter += hit;
            hurt_box.enabled = true;
        }
        else{
            feedback.trigger_enter -= hit;
            hurt_box.enabled = false;
            hit_creatures.Clear(); // clear hit creatures for next enable.
        }
    }
    public float get_self_knockback_force() => self_knockback_force;
    public float get_self_knockback_duration() => self_knockback_duration;

    public void hit(Collider2D other){
        Creature direct = other.GetComponent<Creature>(); // direct creature reference from smaller enemies with one collider; like human characters.
        CreatureLink link = other.GetComponent<CreatureLink>(); // creature linker for bigger creatures with multiple colliders and segments.
        Creature creature = (direct != null)? direct : link.get_creature();
        string name = creature.gameObject.name;
        if(hit_creatures.ContainsKey(creature) == true)
            return;
        creature.get_health().damage(damage_data, knockback_data);
        // add the creature to hit creatures;
        hit_creatures.Add(creature, true);
        hit_creature?.Invoke();
        self_knockback?.Invoke(new KnockbackData(self_knockback_force, self_knockback_duration, other.transform));
    }
}