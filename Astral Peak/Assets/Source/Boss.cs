using System;
using System.Collections;
using UnityEngine;

public abstract class Boss : CreatureInheritor<CharacterMovement>{
    [SerializeField] protected AnimatorOverride animator;
    [SerializeField] protected BossCombat combat;
    [SerializeField] protected ParticlesHandler particles;
    [SerializeField] protected Transform target;
    [SerializeField] int 
        current_pahse,
        max_phase;
    protected Coroutine state;

    protected float dist_to_target() => (transform.position - target.position).x;

    protected void state_switch(IEnumerator n_state){
        StopAllCoroutines();
        state = StartCoroutine(n_state);
        movement.stop();
    }

    public void flip_to_target(){
        if(dist_to_target() < 0)
            flip_right();
        else
            flip_left();
    }

    protected virtual IEnumerator none(){
        yield break;
    }

    protected virtual IEnumerator follow(){
        while(true){
            float dist = dist_to_target();

            // attempt an attack.
            // keep this at the end of the co routine so the ai can stop moving.
            if(combat.cooldown == 0){
                BossAttack chosen_attack = combat.chose_attack(dist);
                if(chosen_attack != null){
                    state_switch(attack(chosen_attack));
                    yield break;
                }
            }

            // if we are not moving right, move right.
            if(dist < 0 && movement.get_move_direction() != new Vector2(1,0)){
                movement.stop();
                movement.move_right(true);
            }
            // if we are not moving left, move left.
            if(dist > 0 && movement.get_move_direction() != new Vector2(-1,0)){
                movement.stop();
                movement.move_left(true);
            }

            // Fixed Update Modifier.
            yield return new WaitForFixedUpdate();
        }
    }

    protected IEnumerator attack(BossAttack attack){
        animator.Play(attack.animation_id);
        yield break;
    }

    protected override void link_events(){
        base.link_events();
        flipped_left                += particles.flip_left;
        flipped_right               += particles.flip_right;
    }

    protected override void unlink_events(){
        base.unlink_events();
        flipped_left                -= particles.flip_left;
        flipped_right               -= particles.flip_right;
    }
}
