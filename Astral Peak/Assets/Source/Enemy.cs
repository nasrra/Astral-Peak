using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;


// for enemy path follow, Hollow knight makes it so that they are restricted to the platform they are placed on.
// this is done with colliders at the edge of platforms, forbiding the ai off their section.
// It's not really noticeable as when they hit the wall they immediately run back at the player.
// This idea should be followed, to mitigate any bugs and anymore time on needless ai path finding.

public class Enemy : Creature{
    public event Action<EnemyState> state_change;

    [Header("Enemy")]
    [SerializeField] protected EnemyState state;
    [SerializeField] protected AiPathFollow path_follow;

    void Start() => set_state(EnemyState.IDLE);

    public void set_state(EnemyState s) => state_change?.Invoke(state = s);

    public void follow_path(bool x) => path_follow.enabled = x;
}

public enum EnemyState{
    ATTACK, // attack player state
    CHASE,  // chase player state
    RETURN, // return to idle state
    IDLE,   // idle pathing state
}