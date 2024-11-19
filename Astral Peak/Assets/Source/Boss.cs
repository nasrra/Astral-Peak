using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public abstract class Boss : CreatureInheritor<CharacterMovement>{
    [SerializeField] protected BossSpriteHandler sprite;
    [SerializeField] protected AnimatorOverride animator;
    [SerializeField] protected BossCombat combat;
    [SerializeField] protected Transform target;
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
            if(chose_attack(dist) == true)
                yield break;
            move_to_player(dist);
            // Fixed Update Modifier.
            yield return new WaitForFixedUpdate();
        }
    }

    protected bool chose_attack(float dist){
        if(combat.cooldown == 0){
            BossAttack chosen_attack = combat.chose_attack(dist);
            if(chosen_attack != null){
                state_switch(attack(chosen_attack));
                return true;
            }
        }
        return false;
    }

    protected void move_to_player(float dist){
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
    }

    protected IEnumerator attack(BossAttack attack){
        animator.Play(attack.animation_id);
        yield break;
    }
}
