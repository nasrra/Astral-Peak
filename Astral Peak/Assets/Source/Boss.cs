using System.Collections;
using System.Collections.Generic;
using DocumentFormat.OpenXml.Wordprocessing;
using UnityEngine;

public abstract class Boss<T> : CreatureInheritor<T> where T : Movement{
    [Header("Boss")]
    [SerializeField] protected BossSpriteHandler sprite;
    [SerializeField] protected Animator animator;
    [SerializeField] protected ParticleHandler particles;
    [SerializeField] protected RangedHolsterHandler ranged;
    [SerializeField] protected MeleeHolsterHandler melee;
    [SerializeField] public AudioPlayer sound;
    [SerializeField] protected List<Collider2D> body_colliders = new List<Collider2D>();
    [SerializeField] protected BossCombat combat;
    [SerializeField] protected Transform target;
    protected Coroutine state;

    protected float dist_to_target() => (transform.position - target.position).x;

    protected virtual void state_switch(IEnumerator n_state){
        movement.stop();
        if(state != null)
            StopCoroutine(state);
        state = StartCoroutine(n_state);
        Debug.Log(2);
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

    protected virtual IEnumerator follow_and_attack(){
        while(true){
            float dist = dist_to_target();
            // attempt an attack.
            if(combat != null && chose_attack(dist) == true)
                yield break;
            move_to_player(dist);
            // Fixed Update Modifier.
            yield return new WaitForFixedUpdate();
        }
    }

    protected virtual IEnumerator follow_only(){
        while(true){
            float dist = dist_to_target();
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

    // disables body colliders so the player cant hit it anymore.
    protected void enable_body_colliders(int flag){
        foreach(Collider2D c in body_colliders)
            c.enabled = flag == 1;
    }

    public void set_target(Transform _target) => target = _target;
}
