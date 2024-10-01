using System;
using System.Security.Cryptography;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Video;


// for enemy path follow, Hollow knight makes it so that they are restricted to the platform they are placed on.
// this is done with colliders at the edge of platforms, forbiding the ai off their section.
// It's not really noticeable as when they hit the wall they immediately run back at the player.
// This idea should be followed, to mitigate any bugs and anymore time on needless ai path finding.

public class Enemy : CreatureInheritor<CharacterMovement>{
    //public event Action<EnemyState> state_change;

    [Header("Enemy")]
    [SerializeField] protected EnemyState state;
    [SerializeField] protected AiPathFollow path_follow;
    [SerializeField] protected AiCombat combat;

    void Start() => link_events();

    void OnDestroy() => unlink_events(); 

    public void combat_state(){
        state = EnemyState.COMBAT;
        path_follow.set_state(AiPathFollowState.NONE);
        combat.set_state(AiCombatState.CHASE);
    }

    public void passive_state(){
        state = EnemyState.PASSIVE;
        path_follow.set_state(AiPathFollowState.RETREAT);
        combat.set_state(AiCombatState.NONE);
    }

    public void stun_state(){
        state = EnemyState.STUN;
        combat.unlink_events(); // to prevent the ai from chasing once stunned.
        path_follow.set_state(AiPathFollowState.NONE);
        combat.set_state(AiCombatState.NONE);
    }

    protected override void link_events(){
        base.link_events();
        combat.target_in_range += combat_state;
        combat.target_left_range += passive_state;
        health.on_guard_broken += stun_state;
    }

    protected override void unlink_events(){
        base.unlink_events();
        combat.target_left_range -= passive_state;
        combat.target_in_range -= combat_state; 
        health.on_guard_broken -= stun_state;
    }
}

public enum EnemyState{
    PASSIVE,
    COMBAT,
    STUN,
}