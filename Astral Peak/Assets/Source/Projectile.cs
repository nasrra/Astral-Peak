using System.Collections.Generic;
using UnityEngine;
public abstract class Projectile : MonoBehaviour{
    [Header("Projectile")]
    [SerializeField] List<Collider2D> colliders = new List<Collider2D>();
    [SerializeField] List<SpriteRenderer> sprites = new List<SpriteRenderer>();
    [SerializeField] protected SimpleMovement movement;
    public abstract void destroy();
    protected void enable_colliders(bool enabled){
        foreach(Collider2D col in colliders)
            col.enabled = enabled;
    }
    protected void enable_sprites(bool enabled){
        foreach(SpriteRenderer sprite in sprites)
            sprite.enabled = enabled;
    }
    protected void damage_creature(Creature creature) => creature.get_health().damage(new DamageData(1), new KnockbackData(20, 0.25f, transform));
    protected void damage_creature_and_self_destruct(Creature creature){
        creature.get_health().damaged += destroy;
        creature.get_health().damage(new DamageData(1), new KnockbackData(20, 0.25f, transform));
        creature.get_health().damaged -= destroy;
    }
}
