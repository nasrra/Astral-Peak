using System;
using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using Entropek.Collections;
using UnityEngine;
using Entropek;

public abstract class Boss<T> : CreatureInheritor<T> where T : Movement{
    public event Action<string> phase_transition, phase_entered, phase_exited;
    [Header("Boss")]
    [SerializeField] protected SerializedDictionary<string, MovementData> movement_presets = new SerializedDictionary<string, MovementData>();    
    [SerializeField] protected SerializedDictionary<string, HealthData> health_presets = new SerializedDictionary<string, HealthData>();    
    [SerializeField] protected List<Collider2D> body_colliders = new List<Collider2D>();
    [SerializeField] protected BossSpriteHandler sprites;
    [SerializeField] public AnimatorOverride animator;
    [SerializeField] protected ParticleHandler particles;
    [SerializeField] protected RangedHolsterHandler ranged;
    [SerializeField] protected MeleeHolsterHandler melee;
    // [SerializeField] public AudioPlayer sound;
    [SerializeField] protected BossCombat combat;
    [SerializeField] protected LightingHandler lighting;
    [SerializeField] protected Transform target;
    [SerializeField] protected string current_phase;
    protected StateQueue state  = new StateQueue(null, null);
    

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
        combat.halt();
        movement.halt();        
        animator.Play(attack.animation_id);
    }

    // NOTE: always do combat first then movement, so that back attacks are chosen :)
    public void follow_and_attack_state(){
        combat.chose_attack_state(target);
        // edge case to check if we are able to attack coming out of an attack.
        if(combat.is_attacking == false)
            movement.move_to(target);
    }

    public void follow_only_state(){
        combat.halt();
        movement.move_to(target);
    }

    public void no_state(){
        combat.halt();
        movement.clear_state();
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

    protected void play_weapon_flash(string weapon){
        sprites.play_charged_flash(weapon);
        // sound.play_sound("ping");
    }

    protected void play_weapon_flash(List<string> weapons){
        sprites.play_charged_flash(weapons);
        // sound.play_sound("ping");
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
    protected virtual void set_phase_data(string phase){
        combat.set_moveset(phase);
        movement.set_data(movement_presets[phase]);
        health.set_data(health_presets[phase]);
        current_phase = phase;
    }
    public void link_phase(string phase){
        Log.MethodCall();
        get_phase_linker()[phase]();
        if(phase.Contains("phase"))
            combat.set_moveset(phase);
        phase_entered?.Invoke(phase);
    }
    public void unlink_phase(string phase){
        get_phase_unlinker()[phase]();
        phase_exited?.Invoke(phase);
    }
    protected virtual Dictionary<string, Action> get_phase_unlinker(){throw Log.MethodNotImplemented(this);}
    protected virtual Dictionary<string, Action> get_phase_linker(){throw Log.MethodNotImplemented(this);}

    protected virtual void stop_all(){
        movement.halt();
        combat.halt();
        combat.renew();
        state.stop();
        particles.stop_all_particles();
        // sound.stop_all_loops();
        lighting.set_intensity(0);
        sprites.renew();     
    }

    public string get_phase() => current_phase;
    public void transition_phase() => phase_transition?.Invoke(current_phase);
    protected virtual void create_phase_linkage(){Log.MethodNotImplemented(this);}
    protected virtual void link_movement(){
        flipped_left  += movement.flip_left;
        flipped_right += movement.flip_right;
        movement.set_flip(flipped);
    }
    protected virtual void unlink_movement(){
        flipped_left  -= movement.flip_left;
        flipped_right -= movement.flip_right;        
    }
    protected virtual void link_combat(){
        flipped_left  += combat.flip_left;
        flipped_right += combat.flip_right;        
        combat.set_flip(flipped);
    }
    protected virtual void unlink_combat(){
        flipped_left  -= combat.flip_left;
        flipped_right -= combat.flip_right;        
    }

    protected virtual void link_particles(){
        flipped_left  += particles.flip_particles_left;
        flipped_right += particles.flip_particles_right;
    }

    protected virtual void unlink_particles(){
        flipped_left  -= particles.flip_particles_left;
        flipped_right -= particles.flip_particles_right;
    }
}
