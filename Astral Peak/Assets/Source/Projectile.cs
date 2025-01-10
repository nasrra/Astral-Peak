using System;
using System.Collections.Generic;
using UnityEngine;
public abstract class Projectile : MonoBehaviour{
    [Header("Projectile")]
    [SerializeField] List<Collider2D> colliders = new List<Collider2D>();
    [SerializeField] List<SpriteRenderer> sprites = new List<SpriteRenderer>();
    [SerializeField] protected SimpleMovement movement;
    public abstract void destroy();
    void OnDestroy()=>ProjectileManager.instance.remove(this);
    protected virtual void Start() => ProjectileManager.instance.add(this);
    
    protected void enable_colliders(bool enabled){
        #if UNITY_EDITOR
        if(colliders.Count <= 0)
            throw new Exception("colliders is empty!");
        #endif
        foreach(Collider2D col in colliders)
            col.enabled = enabled;
    }
    protected void enable_sprites(bool enabled){
        #if UNITY_EDITOR
        if(sprites.Count <= 0)
            throw new Exception("sprites is empty!");
        #endif
        foreach(SpriteRenderer sprite in sprites)
            sprite.enabled = enabled;
    }
    protected void damage_creature(Creature creature) => creature.get_health().damage(new DamageData(1), new KnockbackData(20, 0.25f, transform));
    protected void damage_creature_and_self_destruct(Creature creature){
        creature.get_health().damaged += destroy;
        creature.get_health().damage(new DamageData(1), new KnockbackData(20, 0.25f, transform));
        creature.get_health().damaged -= destroy;
    }
    protected void snap_to_floor(){
        RaycastHit2D hit;
        hit = Physics2D.Raycast(transform.position, Vector2.down, Mathf.Infinity, LayersManager.BITWISE_GROUND);
        if(hit==true)
            transform.position = hit.point;
    }
}
