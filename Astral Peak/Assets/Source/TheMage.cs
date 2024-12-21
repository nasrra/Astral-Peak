using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class TheMage : Boss<Movement>{
    // Base: 
    void Awake(){
        link_events();
        sound.set_functions(new MageSound(sound));
    }

    void Start(){
        state_switch(idle(2));
        //StartCoroutine(test());
    }

    void OnDestroy(){
        unlink_health();
    }





    //
    IEnumerator lock_idle(){
        animator.Play("idle");
        yield break;
    }

    void idle_state(float x) => state_switch(idle(x));
    IEnumerator idle(float x){
        animator.Play("idle");
        yield return new WaitForSeconds(x);
        state_switch(follow());
        yield break;
    }

    protected override IEnumerator follow(){
        animator.Play("walk");
        return base.follow();
    }

    // Linkage:
    protected void link_events(){
        link_health();
        link_movement();
        link_combat();
    }

    protected void unlink(){
        unlink_health();
        unlink_movement();
        unlink_combat();
    }

    void link_health() => health.damaged += sprite.play_damaged_flash;
    void unlink_health() => health.damaged -= sprite.play_damaged_flash;
    void link_movement() => get_movement().move_direction_changed += face_move_dir;
    void unlink_movement() => get_movement().move_direction_changed -= face_move_dir;
    void link_combat() => combat.attack_ended += idle_state;
    void unlink_combat() => combat.attack_ended -= idle_state;
}
