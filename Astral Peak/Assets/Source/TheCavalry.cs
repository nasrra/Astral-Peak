using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheCavalry : CreatureInheritor<Movement>{
    // Start is called before the first frame update
    [SerializeField] BossCombat combat;
    [SerializeField] Animator animator;
    [SerializeField] Transform target;
    [SerializeField] float target_dist;
    Coroutine state;
    
    private readonly int IDLE = Animator.StringToHash("idle");
    private readonly int RUN = Animator.StringToHash("run");

    void Start(){
        combat.moveset = new List<BossAttack>(){
            new BossAttack(Animator.StringToHash("front_strike"),8,4),
            //new BossAttack(Animator.StringToHash("back_strike"),8,4),
            new BossAttack(Animator.StringToHash("bite1"),8, 4),
        };
        state_switch(idle());
        link_events();
    } 

    void OnDestroy() => unlink_events();

    float dist_to_target() => (transform.position - target.position).x;
    // Used for animation key events: when attack animations have ended.
    public void attack_ended() => state_switch(idle());

    void state_switch(IEnumerator n_state){
        StopAllCoroutines();
        state = null;
        movement.stop();
        state = StartCoroutine(n_state);
    }

    IEnumerator follow(){
        animator.Play(RUN);
        while(true){
            target_dist = dist_to_target();

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
            // attempt an attack.
            // keep this at the end of the co routine so the ai can stop moving.
            if(combat.cooldown == 0)
                state_switch(attack(combat.chose_attack(target_dist)));

            // Fixed Update Modifier.
            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator attack(BossAttack attack){
        animator.Play(attack.animation_id);
        yield break;
    }

    IEnumerator idle(){
        animator.Play(IDLE);
        yield return new WaitForSeconds(1);
        state_switch(follow());
        yield break;
    }
}
