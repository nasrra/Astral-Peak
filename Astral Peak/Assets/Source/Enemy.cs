using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



// for enemy path follow, Hollow knight makes it so that they are restricted to the platform they are placed on.
// this is done with colliders at the edge of platforms, forbiding the ai off their section.
// It's not really noticeable as when they hit the wall they immediately run back at the player.
// This idea should be followed, to mitigate any bugs and anymore time on needless ai path finding.

public class Enemy : CreatureInheritor<CharacterMovement>{
    public event Action<Enemy> enemy_death;
    [Header("Enemy")]
    [field: SerializeField] public AnimatorOverride animator {get; private set;}
    [field: SerializeField] public ParticleHandler particles {get; private set;}
    [field: SerializeField] public BossSpriteHandler sprites {get; private set;}
    [field: SerializeField] public AudioPlayer sound {get; private set;}
    [SerializeField] protected List<MovementPath> paths = new List<MovementPath>();
    [SerializeField] protected List<Collider2D> body_colliders = new List<Collider2D>();
    [SerializeField] Animator summoning_animator;
    [SerializeField] public Transform origin, target;

    public void play_summoning_animation() => summoning_animator.Play("turn_on");
    public void stop_summoning_animation() => summoning_animator.Play("turn_off");

    protected virtual void Start(){
        EnemyManager.instance.add(this);
    }

    protected override void state_switch(ref Coroutine state, IEnumerator _state){
        movement.clear_move_direction();
        base.state_switch(ref state, _state);
    }

    protected float dist_to_target() => (transform.position - target.position).x;


    public void flip_to_target(){
        if(dist_to_target() < 0)
            flip_right();
        else
            flip_left();
    }

    protected void enable_body_colliders(int x){
        foreach(Collider2D col in body_colliders)
            col.enabled = x == 1;
    }

    protected void set_body_colliders_exclude_layers(int bitwise_layer){
        foreach(Collider2D c in body_colliders)
            c.excludeLayers = bitwise_layer;
    }

    protected override void death_complete(){
        enemy_death?.Invoke(this);
        EnemyManager.instance?.remove(this);
        base.death_complete();
    }

    protected virtual void link_movement(){
        flipped_left  += movement.flip_left;
        flipped_right += movement.flip_right;
        movement.set_flip(flipped);
    }
    protected virtual void unlink_movement(){
        flipped_left  -= movement.flip_left;
        flipped_right -= movement.flip_right;        
    }
}