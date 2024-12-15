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
    protected event Action<MovementOption> pathing_movement;
    [SerializeField] protected List<AiPath> paths = new List<AiPath>();
    [SerializeField] protected Transform origin;

    public IEnumerator pathing_loop(){
        AiPath current_path;
        int path_index = 0;

        while(true){
            // start movement.
            current_path = paths[path_index];
            movement.movement(current_path.movement, true);
            pathing_movement?.Invoke(current_path.movement);
            yield return new WaitForSeconds(current_path.duration);
            
            movement.stop();
            path_index = ((path_index + 1) >= paths.Count)? 0 : path_index + 1;
            
            yield return null;
        }
    }

    public override void kill(){
        enemy_death?.Invoke(this);
        base.kill();
    }

    public IEnumerator retreat_loop(){
        animator.Play(HollowAnimator.WALK);
        float curr_dist = dist_to_target();
        while(Mathf.Abs(curr_dist) >= 0.25f){
            curr_dist = dist_to_target();
            // if we are not moving right, move right.
            if(curr_dist < 0 && movement.get_move_direction() != new Vector2(1,0))
                movement.move_right(true);
            // if we are not moving left, move left.
            if(curr_dist > 0 && movement.get_move_direction() != new Vector2(-1,0))
                movement.move_left(true);
            yield return null;
        }
        state_switch(pathing_loop());
        yield break;
        float dist_to_target() => (transform.position - origin.position).x;
    }
}

// Path that the Ai will follow.
[System.Serializable]
public struct AiPath{
    // direction to move in.
    public MovementOption movement;
    // duration of movement.
    public float duration;
}