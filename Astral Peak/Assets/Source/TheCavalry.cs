using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheCavalry : CreatureInheritor<CharacterMovement>{
    // Start is called before the first frame update
    [SerializeField] BossCombat combat;
    [SerializeField] CavalryAnimator animator;
    [SerializeField] Transform target;
    [SerializeField] float target_dist;
    Coroutine state;
    
    private readonly int
        IDLE = Animator.StringToHash("idle"),
        RUN = Animator.StringToHash("run"),
        BITE_2 = Animator.StringToHash("bite_2"),
        BITE_3 = Animator.StringToHash("bite_3");

    void Start(){
        state_switch(idle());
        link_events();
    } 

    void OnDestroy() => unlink_events();

    float dist_to_target() => (transform.position - target.position).x;
    // Used for animation key events: when attack animations have ended.
    public void attack_ended() => state_switch(idle());

    void state_switch(IEnumerator n_state){
        StopAllCoroutines();
        state = StartCoroutine(n_state);
        movement.stop();
    }

    // animator events.
    public void back_strike_jump() => movement.dash(transform.rotation.y == 0? Vector2.left : Vector2.right, 15, 0.55f);
    public void second_bite_lunge() => movement.dash(transform.rotation.y == 0? Vector2.right : Vector2.left, 5, 0.2f);
    public void second_bite(){
        if(Random.Range(0,11) > 2)
            animator.play(BITE_2);
    }
    public void third_bite(){
        if(Random.Range(0,11) > 2)
            animator.play(BITE_3);       
    }

    IEnumerator follow(){
        animator.play(RUN);
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

    IEnumerator idle(){
        animator.play(IDLE);
        yield return new WaitForSeconds(1);
        state_switch(follow());
        yield break;
    }

    protected override void link_events(){
        base.link_events();
        flipped_left += combat.flip_particles_left;
        flipped_right += combat.flip_particles_right;
    }

    protected override void unlink_events(){
        base.link_events();
        flipped_left -= combat.flip_particles_left;
        flipped_right -= combat.flip_particles_right;
    }
}
