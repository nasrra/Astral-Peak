using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class TheCavalry : Boss{
    public static TheCavalry instance;

    // Start is called before the first frame update
    [SerializeField] CavalryRangedCombatHandler ranged;
    [SerializeField] CavalryMeleeCombatHandler melee;
    [SerializeField] SpriteHandler sprite;
    [SerializeField] FetchSword fetch_sword;
    [SerializeField] float
        follow_speed,
        follow_fsword_speed;

    void Awake(){
        instance = this;
        state_switch(idle(1));
        link_events();
    } 

    void OnDisable() => unlink_events();

    // animator events.
    public void back_strike_forward_leap() => movement.dash(transform.rotation.y == 0? Vector2.right : Vector2.left, 20, 0.75f);
    public void back_strike_backward_jump() => movement.dash(transform.rotation.y == 0? Vector2.left : Vector2.right, 20, 0.55f);
    public void jump_away_dash() => movement.dash(transform.rotation.y == 0? Vector2.left : Vector2.right, 20, 0.35f);
    public void second_bite_lunge() => movement.dash(transform.rotation.y == 0? Vector2.right : Vector2.left, 20, 0.2f);
    public void switch_to_move_to_fetch_sword() => state_switch(follow_fetch_sword());
    public void switch_to_idle_no_sword() => state_switch(idle_no_sword());
    public void switch_to_idle(float x) => state_switch(idle(x));
    public void ground_slam_camera_zoom() => CameraController.instance.zoom_out_state(18, 4f);
    public void ground_slam_camera_reset() => CameraController.instance.reset_zoom_state(32f);
    public void sword_summon_camera_zoom() => CameraController.instance.zoom_out_state(14, 2f);
    public void sword_summon_camera_reset() => CameraController.instance.reset_zoom_state(1f);

    // states: 
    public override void enter_cutscene_state() => state_switch(lock_idle());
    public override void exit_cutscene_state() => state_switch(idle(1));

    IEnumerator idle(float x){
        animator.Play(CavalryAnimator.IDLE);
        yield return new WaitForSeconds(x);
        state_switch(follow());
        yield break;
    }

    IEnumerator lock_idle(){
        animator.Play(CavalryAnimator.IDLE);
        yield break;
    }

    IEnumerator idle_no_sword(){
        animator.Play(CavalryAnimator.NO_SWORD_IDLE);
        yield break;
    }

    IEnumerator pickup_sword(){
        animator.Play(CavalryAnimator.PICKUP_SWORD);
        Destroy(fetch_sword.gameObject);
        target = Player.player.transform;
        yield return new WaitForSeconds(1);
        state_switch(follow());
        yield break;
    }

    void fetch_sword_landed(){
        animator.Play(CavalryAnimator.WHISTLE);
    }

    protected override IEnumerator follow(){
        animator.Play(CavalryAnimator.RUN);
        movement.set_speed(follow_speed);
        state_switch(base.follow());
        yield break;
    }

    IEnumerator follow_fetch_sword(){
        target = fetch_sword.transform;
        movement.set_speed(follow_fsword_speed);
        animator.Play(CavalryAnimator.NO_SWORD_RUN);
        while(true){
            
            float dist = dist_to_target();
            
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

            if(Mathf.Abs(dist) <= 0.5f){
                state_switch(pickup_sword());
                yield break;
            }
            yield return null;
        }
    }

    void link_fetch_sword(GameObject sword){
        fetch_sword = sword.GetComponent<FetchSword>();
        fetch_sword.landed += fetch_sword_landed;
        fetch_sword.landed += unlink_fetch_sword;
    }
    void unlink_fetch_sword() => fetch_sword.landed -= fetch_sword_landed;

    protected override void link_events(){
        base.link_events();
        combat.attack_ended         += switch_to_idle;
        ranged.fetch_sword_fired    += link_fetch_sword;
        health.damaged              += sprite.play_damaged_flash;
    }

    protected override void unlink_events(){
        base.link_events();
        combat.attack_ended         -= switch_to_idle;
        ranged.fetch_sword_fired    -= link_fetch_sword;
        health.damaged              -= sprite.play_damaged_flash;
    }
}
