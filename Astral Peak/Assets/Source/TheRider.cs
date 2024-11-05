using System.Collections;
using UnityEngine;

public class TheRider : Boss{
    public static TheRider instance;

    [SerializeField] RiderRangedCombat ranged;
    public void forward_strike_lunge() => movement.dash(transform.rotation.y == 0? Vector2.right : Vector2.left, 20, 0.30f);

    void Awake(){
        instance = this;
        state_switch(idle(2));
        link_events();
    }

    void OnDisable() => unlink_events();

    public void switch_to_idle(float x) => state_switch(idle(x));
    IEnumerator idle(float x){
        animator.Play(RiderAnimator.IDLE);
        yield return new WaitForSeconds(x);
        state_switch(follow());
        yield break;
    }

    protected override IEnumerator follow(){
        animator.Play(RiderAnimator.RUN);
        state_switch(base.follow());
        yield break;
    }

    protected override void link_events(){
        base.link_events();
        combat.attack_ended         += switch_to_idle;
    }

    protected override void unlink_events(){
        base.unlink_events();
        combat.attack_ended         -= switch_to_idle;
    }

}
