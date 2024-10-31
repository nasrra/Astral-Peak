using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem.Switch;

public class TheCavalry : CreatureInheritor<CharacterMovement>{
    // Start is called before the first frame update
    [SerializeField] BossCombat combat;
    [SerializeField] CavalryRangedCombat ranged;
    [SerializeField] CavalryAnimator animator;
    [SerializeField] Transform target;
    [SerializeField] float target_dist;
    [SerializeField] FetchSword fetch_sword;
    Coroutine state;

    void Start(){
        state_switch(idle());
        link_events();
    } 

    void OnDestroy() => unlink_events();

    float dist_to_target() => (transform.position - target.position).x;

    void state_switch(IEnumerator n_state){
        StopAllCoroutines();
        state = StartCoroutine(n_state);
        movement.stop();
    }

    // animator events.
    public void back_strike_jump() => movement.dash(transform.rotation.y == 0? Vector2.left : Vector2.right, 15, 0.55f);
    public void second_bite_lunge() => movement.dash(transform.rotation.y == 0? Vector2.right : Vector2.left, 5, 0.2f);

    IEnumerator follow(){
        animator.play(CavalryAnimator.RUN);
        while(true){
            target_dist = dist_to_target();

            // attempt an attack.
            // keep this at the end of the co routine so the ai can stop moving.
            if(combat.cooldown == 0){
                BossAttack chosen_attack = combat.chose_attack(target_dist);
                if(chosen_attack != null){
                    state_switch(attack(chosen_attack));
                    yield break;
                }
            }

            // if we are not moving right, move right.
            if(target_dist < 0 && movement.get_move_direction() != new Vector2(1,0)){
                movement.stop();
                movement.move_right(true);
            }
            // if we are not moving left, move left.
            if(target_dist > 0 && movement.get_move_direction() != new Vector2(-1,0)){
                movement.stop();
                movement.move_left(true);
            }

            // Fixed Update Modifier.
            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator attack(BossAttack attack){
        animator.play(attack.animation_id);
        yield break;
    }

    public void switch_to_idle() => state_switch(idle());
    IEnumerator idle(){
        animator.play(CavalryAnimator.IDLE);
        yield return new WaitForSeconds(1);
        state_switch(follow());
        yield break;
    }

    public void switch_to_idle_no_sword() => state_switch(idle_no_sword());
    IEnumerator idle_no_sword(){
        animator.play(CavalryAnimator.NO_SWORD_IDLE);
        yield break;
    }

    public void switch_to_move_to_fetch_sword() => state_switch(move_to_fetch_sword());
    IEnumerator move_to_fetch_sword(){
        target = fetch_sword.transform;
        while(true){
            target_dist = dist_to_target();
            animator.play(CavalryAnimator.NO_SWORD_RUN);

            if(Mathf.Abs(target_dist) <= 0.5f){
                target = Player.player.transform;
                Destroy(fetch_sword.gameObject);
                state_switch(follow());
                yield break;
            }
            
            // if we are not moving right, move right.
            if(target_dist < 0 && movement.get_move_direction() != new Vector2(1,0)){
                movement.stop();
                movement.move_right(true);
            }
            // if we are not moving left, move left.
            if(target_dist > 0 && movement.get_move_direction() != new Vector2(-1,0)){
                movement.stop();
                movement.move_left(true);
            }

            yield return null;
        }
    }

    void link_fetch_sword(GameObject sword){
        fetch_sword = sword.GetComponent<FetchSword>();
        fetch_sword.landed += animator.whistle;
        fetch_sword.landed += unlink_fetch_sword;
    }
    void unlink_fetch_sword() => fetch_sword.landed -= animator.whistle;

    protected override void link_events(){
        base.link_events();
        flipped_left        += combat.flip_particles_left;
        flipped_right       += combat.flip_particles_right;
        combat.attack_ended += switch_to_idle;
        ranged.fetch_sword_fired += link_fetch_sword;
    }

    protected override void unlink_events(){
        base.link_events();
        flipped_left        -= combat.flip_particles_left;
        flipped_right       -= combat.flip_particles_right;
        combat.attack_ended -= switch_to_idle;
        ranged.fetch_sword_fired -= link_fetch_sword;
    }
}
