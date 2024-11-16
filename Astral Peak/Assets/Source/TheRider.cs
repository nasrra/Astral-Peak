using System.Collections;
using UnityEngine;

public class TheRider : Boss{
    public static TheRider instance;

    [SerializeField] RiderRangedCombat ranged;
    public void forward_strike_lunge() => movement.dash(transform.rotation.y == 0? Vector2.right : Vector2.left, 20, 0.30f);
    public void signature_strike_lunge() => movement.dash(transform.rotation.y == 0? Vector2.right : Vector2.left, 20, 0.30f);
    public void jump_n_dash_jump_back() => movement.dash(transform.rotation.y == 0? Vector2.left : Vector2.right, 20, 0.25f);
    public void jump_n_dash_front_leap() => movement.dash(transform.rotation.y == 0? Vector2.right : Vector2.left, 40, 0.30f);

    void OnEnable(){
        instance = this;
        state_switch(yell());
        link_events();
    }

    void OnDisable() => unlink_events();

    public override void enter_cutscene_state() => state_switch(lock_idle());
    public override void exit_cutscene_state() => state_switch(idle(1));

    public void switch_to_idle(float x) => state_switch(idle(x));
    IEnumerator idle(float x){
        animator.Play(RiderAnimator.IDLE);
        yield return new WaitForSeconds(x);
        state_switch(follow());
        yield break;
    }

    IEnumerator yell(){
        animator.Play(RiderAnimator.YELL); 
        yield return new WaitForSeconds(3);
        state_switch(idle(1));
        yield break;
    }

    IEnumerator lock_idle(){
        animator.Play(RiderAnimator.IDLE);
        yield break;
    }

    protected override IEnumerator follow(){
        animator.Play(RiderAnimator.RUN);
        state_switch(base.follow());
        yield break;
    }

    protected void link_events(){
        movement.move_direction_changed         += face_move_dir;
        health.death                            += kill;
        health.death                            += get_movement().StopAllCoroutines;
        combat.attack_ended                     += switch_to_idle;
        health.damaged                          += sprite.play_damaged_flash;
        flipped_left                            += particles.flip_left;
        flipped_right                           += particles.flip_right;
    }

    protected void unlink_events(){
        movement.move_direction_changed         -= face_move_dir;
        health.death                            -= kill;
        health.death                            -= get_movement().StopAllCoroutines;
        combat.attack_ended                     -= switch_to_idle;
        health.damaged                          -= sprite.play_damaged_flash;
        flipped_left                            -= particles.flip_left;
        flipped_right                           -= particles.flip_right;
    }

}
