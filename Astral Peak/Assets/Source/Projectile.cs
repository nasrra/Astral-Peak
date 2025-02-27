using System;
using System.Collections.Generic;
using UnityEngine;
public abstract class Projectile : MonoBehaviour{
    [Header("Projectile")]
    [SerializeField] protected AudioPlayer audio_player;
    [SerializeField] List<Collider2D> colliders = new List<Collider2D>();
    [SerializeField] List<SpriteRenderer> sprites = new List<SpriteRenderer>();
    [SerializeField] protected SimpleMovement movement;
    [SerializeField] bool destroy_on_hit = true;
    protected bool _is_being_destroyed = false; // here to stop multiple calls from OnTriggerEnter when colliding with two colliders simultaneously.

    void OnDestroy()=>ProjectileManager.instance?.remove(this);
    protected virtual void Start() => ProjectileManager.instance?.add(this);
    
    protected virtual void play_sound(){}
    protected virtual void stop_sound(){}
    public virtual void destroy(){
        if(_is_being_destroyed != false)
            return;
        _is_being_destroyed = true;
        stop_sound();
        enable_colliders(false);
        enable_sprites(false);
        movement.zero_velocity();
        movement.StopAllCoroutines();
        Destroy(gameObject, 2);
    }

    protected virtual void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.layer==LayersManager.PLAYER)
            if(destroy_on_hit == true)
                damage_creature_and_self_destruct(other.GetComponent<Creature>());
            else
                damage_creature(other.GetComponent<Creature>());
    }

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
    protected void damage_creature(Creature creature) => creature.health.damage(new DamageData(1), new KnockbackData(20, 0.25f, transform));
    protected void damage_creature_and_self_destruct(Creature creature){
        creature.health.damaged += destroy;
        creature.health.damage(new DamageData(1), new KnockbackData(20, 0.25f, transform));
        creature.health.damaged -= destroy;
    }
    protected void snap_to_floor(){
        RaycastHit2D hit;
        hit = Physics2D.Raycast(transform.position, Vector2.down, Mathf.Infinity, LayersManager.BITWISE_GROUND);
        if(hit==true)
            transform.position = hit.point;
    }
}
