using System;
using System.Collections;
using System.Collections.Generic;
using Deluz;
using DocumentFormat.OpenXml.Wordprocessing;
using UnityEngine;

public abstract class Boss<T> : CreatureInheritor<T> where T : Movement{
    public event Action<int> phase_selected;
    [Header("Boss")]
    [SerializeField] protected BossSpriteHandler sprites;
    [SerializeField] protected List<Collider2D> body_colliders = new List<Collider2D>();
    [SerializeField] public AnimatorOverride animator;
    [SerializeField] protected ParticleHandler particles;
    [SerializeField] protected RangedHolsterHandler ranged;
    [SerializeField] protected MeleeHolsterHandler melee;
    [SerializeField] public AudioPlayer sound;
    [SerializeField] protected BossCombat combat;
    [SerializeField] protected LightingHandler lighting;
    [SerializeField] protected Transform target;
    [SerializeField] protected int phase = 1;
    protected Dictionary<int, Action> phase_linker;
    protected Dictionary<int, Action> phase_unlinker;

    public void select_phase(int _phase){
        int previous_phase = phase-1;
        if(previous_phase>0)
            unlink_phase(previous_phase);
        link_phase(_phase);        
        phase_selected(phase = _phase);
    }
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

    protected override void entered_game_state(GameState state){
        if(state == GameState.CUTSCENE)
            enter_cutscene_state();
    }
    protected override void exited_game_state(GameState state){
        if(state == GameState.CUTSCENE)
            exit_cutscene_state();
    }
    protected void link_phase_select(){
        phase_selected += link_phase;
        phase_selected += unlink_phase;
        phase_selected += combat.set_moveset;
    }
    protected void unlink_phase_select(){
        phase_selected -= link_phase;
        phase_selected -= unlink_phase;
        phase_selected -= combat.set_moveset;
    }
    private void link_phase(int phase) => phase_linker[phase]();
    private void unlink_phase(int phase) => phase_unlinker[phase]();
    protected virtual void create_phase_linkage(){Log.MethodNotImplemented(this);}
}
