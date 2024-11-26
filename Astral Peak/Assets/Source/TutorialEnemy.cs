using System.Collections;
using UnityEngine;

public class TutorialEnemy : Enemy{

    bool target_in_range;
    float stun_state_timer = 2f;

    void Start(){
        link_events();
        state_switch(pathing_loop());
    }

    void OnDestroy(){
        unlink_events();
    }

    void idle_state(float x) => state_switch(idle(x));
    IEnumerator idle(float x){
        animator.Play(RiderAnimator.IDLE);
        yield return new WaitForSeconds(x);
        state_switch(follow());
        yield break;
    }

    void follow_state() => state_switch(follow());
    protected override IEnumerator follow(){        
        animator.Play(HollowAnimator.IDLE);
        state_switch(base.follow());
        yield break;
    }

    public override void kill(){
        base.kill();
        Destroy(gameObject);
    }

    public void stun_state() =>  state_switch(stun_state_loop());
    protected IEnumerator stun_state_loop(){
        // to prevent the ai from chasing once staggered.
        unlink_combat();
        // interupt attack animation.
        animator.Play(HollowAnimator.STUNNED);

        yield return new WaitForSeconds(stun_state_timer);

        link_combat();
        recovery_state();
        yield break;
    }

    public void recovery_state(){
        state_switch(target_in_range? follow() : retreat_loop());
    }

    #region linkage
    protected void link_events(){
        link_combat();
        link_health();
        link_movement();
    }

    protected void unlink_events(){
        unlink_combat();
        unlink_health(); 
        unlink_movement();
    }

    void player_in_range(){
        target_in_range = true;
        follow_state();
    }

    void player_left_range(){
        target_in_range = false;
        recovery_state();
    }

    private void link_combat(){
        combat.enabled = true;
        (combat as TutorialEnemyCombat).player_in_range += player_in_range;        
        (combat as TutorialEnemyCombat).player_left_range += player_left_range;
        combat.attack_ended += idle_state;
    }
    private void unlink_combat(){
        (combat as TutorialEnemyCombat).player_in_range -= follow_state;        
        (combat as TutorialEnemyCombat).player_left_range -= recovery_state;
        combat.attack_ended -= idle_state;
        combat.StopAllCoroutines();
        combat.enabled = false;
    }

    private void link_movement(){
        movement.move_direction_changed         += face_move_dir;
    }

    private void unlink_movement(){
        movement.move_direction_changed         -= face_move_dir;
    }

    protected void link_health(){
        health.damaged += stun_state;
        health.damaged += sprite.play_damaged_flash;
        health.death += kill;
    }
    protected void unlink_health(){
        health.damaged -= stun_state;
        health.damaged -= sprite.play_damaged_flash;
        health.death -= kill;
    }
    #endregion
}
