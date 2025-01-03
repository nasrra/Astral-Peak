using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



// for enemy path follow, Hollow knight makes it so that they are restricted to the platform they are placed on.
// this is done with colliders at the edge of platforms, forbiding the ai off their section.
// It's not really noticeable as when they hit the wall they immediately run back at the player.
// This idea should be followed, to mitigate any bugs and anymore time on needless ai path finding.

public class Enemy : Boss<Movement>{
    public event Action<Enemy> enemy_death;
    [Header("Enemy")]
    [SerializeField] protected List<MovementPath> paths = new List<MovementPath>();
    [SerializeField] Animator summoning_animator;
    [SerializeField] protected Transform origin;

    public void play_summoning_animation() => summoning_animator.Play("turn_on");
    public void stop_summoning_animation() => summoning_animator.Play("turn_off");


    public override void kill(){
        enemy_death?.Invoke(this);
        base.kill();
    }
}