using System.Collections;
using UnityEngine;


// for enemy path follow, Hollow knight makes it so that they are restricted to the platform they are placed on.
// this is done with colliders at the edge of platforms, forbiding the ai off their section.
// It's not really noticeable as when they hit the wall they immediately run back at the player.
// This idea should be followed, to mitigate any bugs and anymore time on needless ai path finding.

public class Enemy : CreatureInheritor<CharacterMovement>{
    //public event Action<EnemyState> state_change;

    [Header("Enemy")]
    [SerializeField] protected float stun_state_timer;
    [SerializeField] protected EnemyState state;
    [SerializeField] protected AiPathFollow path_follow;
    [SerializeField] protected AiCombat combat;
    [SerializeField] protected MeleeHolster melee;
    private Coroutine coroutine;

    void Start() => link_events();
    void OnDestroy() => unlink_events(); 

#region States
    protected void state_switch_clean_up(){
        // turn off all states to ensure the next state behaves as intended.
        path_follow.set_state(AiPathFollowState.NONE);
        combat.none_state();
    }

    public void combat_state(){
        state = EnemyState.COMBAT;
        state_switch_clean_up();
        combat.chase_state();
    }

    public void passive_state(){
        state = EnemyState.PASSIVE;
        state_switch_clean_up();
        path_follow.set_state(AiPathFollowState.RETREAT);
    }

    public void stagger_state(){
        state = EnemyState.STAGGER;
        state_switch_clean_up();

        // to prevent the ai from chasing once staggered.
        combat.unlink_internal();
        unlink_combat();

        // interupt attack animation.
        animator.SetTrigger("parried");
    }

    public void stun_state(){
        state = EnemyState.STUN;
        state_switch_clean_up();
        StopAllCoroutines();
        StartCoroutine(stun_state_loop());
    }

    protected IEnumerator stun_state_loop(){
        // to prevent the ai from chasing once staggered.
        unlink_combat();
        combat.unlink_internal();
        // interupt attack animation.
        animator.SetTrigger("parried");

        float timer = stun_state_timer;
        while(timer >= 0.0f){
            timer -= Time.deltaTime;
            yield return null;
        }

        // re-link combat to resume attacks.
        link_combat();
        combat.link_internal();
        recovery_state();
        yield break;
    }

    // add a recovery state later...
    public void recovery_state(){
        state_switch_clean_up();
        combat.recovery_state();
        
        // check if we are able to continue attacking.
        AiCombatState state = combat.get_state();

        // if not then retreat back to path follow loop.
        if(state == AiCombatState.NONE)
            path_follow.set_state(AiPathFollowState.RETREAT);
    }

#endregion
#region Events
    private void attack_failed(Collider2D other){
        movement.knockback(
            transform.position - other.transform.position, 
            melee.get_self_knockback_force(), 
            melee.get_self_knockback_duration()
        );
        damage(
            (int)melee.get_self_damage(), 
            other.gameObject
        );
    }
#endregion
#region linkage
    protected override void link_events(){
        base.link_events();
        link_combat();
        link_guard();
        link_melee();
    }

    protected override void unlink_events(){
        base.unlink_events();
        unlink_combat();
        unlink_guard(); 
        unlink_melee();
    }

    private void link_combat(){
        combat.target_in_range += combat_state;
        combat.target_left_range += passive_state;       
    }

    private void unlink_combat(){
        combat.target_in_range -= combat_state;        
        combat.target_left_range -= passive_state;
    }

    private void link_guard(){
        guard.damaged += stun_state;
        guard.broken += stagger_state;
    }

    private void unlink_guard(){
        guard.damaged -= stun_state;
        guard.broken -= stagger_state;
    }

    private void link_melee(){
        melee.hit_enemy_guard += attack_failed;
    }

    private void unlink_melee(){
        melee.hit_enemy_guard -= attack_failed;
    }
#endregion
}

public enum EnemyState{
    PASSIVE,
    COMBAT,
    STUN,
    STAGGER,
}