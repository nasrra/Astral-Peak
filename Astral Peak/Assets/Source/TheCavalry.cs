using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheCavalry : CreatureInheritor<CharacterMovement>{
    // Start is called before the first frame update
    [SerializeField] BossCombat combat;
    [SerializeField] Animator animator;
    [SerializeField] Transform target;
    [SerializeField] float target_dist;
    [SerializeField] MeleeHolster 
        bite, front_swing, back_swing;
    Coroutine state;
    
    private readonly int
        IDLE = Animator.StringToHash("idle"),
        RUN = Animator.StringToHash("run"),
        BITE_2 = Animator.StringToHash("bite_2"),
        BITE_3 = Animator.StringToHash("bite_3");

    void Start(){
        combat.set_front_moveset(new List<BossAttack>(){
            //new BossAttack(Animator.StringToHash("front_strike"),6,2),
            new BossAttack(Animator.StringToHash("bite_1"),6, 2),
        });

        combat.set_back_moveset(new List<BossAttack>(){
            new BossAttack(Animator.StringToHash("back_strike"),6,2),
        });

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
            animator.Play(BITE_2);
    }
    public void third_bite(){
        if(Random.Range(0,11) > 2)
            animator.Play(BITE_3);       
    }
    public void play_bite_effect() => bite.slash_effect();
    public void play_front_swing_effect() => front_swing.slash_effect();
    public void play_back_swing_effect() => back_swing.slash_effect();

    IEnumerator follow(){
        animator.Play(RUN);
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
        animator.Play(attack.animation_id);
        yield break;
    }

    IEnumerator idle(){
        animator.Play(IDLE);
        yield return new WaitForSeconds(1);
        state_switch(follow());
        yield break;
    }

    public void flip_particles_left(){

    }

    public void flip_particles_right(){

    }

    protected override void link_events(){
        base.link_events();
        flipped_left += flip_particles_left;
        flipped_right += flip_particles_right;
    }

    protected override void unlink_events(){
        base.link_events();
        flipped_left -= flip_particles_left;
        flipped_right -= flip_particles_right;
    }
}
