using System;
using System.Collections;
using System.Collections.Generic;
using DocumentFormat.OpenXml.Wordprocessing;
using UnityEngine;

public abstract class Boss<T> : CreatureInheritor<T> where T : Movement{
    public event Action<int> phase_selected;
    [Header("Boss")]
    [SerializeField] protected int phase = 1;
    [SerializeField] protected BossSpriteHandler sprite;
    [SerializeField] public AnimatorOverride animator;
    [SerializeField] protected ParticleHandler particles;
    [SerializeField] protected RangedHolsterHandler ranged;
    [SerializeField] protected MeleeHolsterHandler melee;
    [SerializeField] public AudioPlayer sound;
    [SerializeField] protected List<Collider2D> body_colliders = new List<Collider2D>();
    [SerializeField] protected BossCombat combat;
    [SerializeField] protected Transform target;

    public void select_phase(int _phase) => phase_selected(phase = _phase);
    protected float dist_to_target() => (transform.position - target.position).x;

    protected override void state_switch(ref Coroutine state, IEnumerator _state){
        movement.clear_move_direction();
        base.state_switch(ref state, _state);
    }

    public void flip_to_target(){
        if(dist_to_target() < 0)
            flip_right();
        else
            flip_left();
    }

    protected virtual void attack(BossAttack attack){
        no_state();
        animator.Play(attack.animation_id);
    }

    // NOTE: always do combat first then movement, so that back attacks are chosen :)
    public void follow_and_attack_state(){
        combat.chose_attack_state(target);
        // edge case to check if we are able to attack coming out of an attack.
        if(combat.is_attacking == false)
            movement.move_to_target_state(target);
    }

    public void follow_only_state(){
        combat.no_state();
        movement.move_to_target_state(target);
    }

    public void no_state(){
        combat.no_state();
        movement.no_state();
    }

    // disables body colliders so the player cant hit it anymore.
    protected void enable_body_colliders(int flag){
        foreach(Collider2D c in body_colliders)
            c.enabled = flag == 1;
    }
    protected void set_body_colliders_exclude_layers(int bitwise_layer){
        foreach(Collider2D c in body_colliders)
            c.excludeLayers = bitwise_layer;
    }

    public void set_target(Transform _target) => target = _target;
}
