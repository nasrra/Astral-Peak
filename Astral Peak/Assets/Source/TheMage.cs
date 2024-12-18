using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class TheMage : Boss<Movement>{

    [Header("The Mage")]
    [SerializeField] ParticlesHandler particles;
    [SerializeField] AnimationEvent animation_event;
    [SerializeField] MageAudio sound;
    Dictionary<string, Action> animation_events;


    // Base: 
    void Awake(){
        link_events();
        create_animation_events();
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

    public void enter_invisible(){
        animator.Play("enter_invisible", 1);
    }

    public void exit_invisible(){
        animator.Play("exit_invisible", 1);
    }
    IEnumerator test(){
        while(true){
            exit_invisible();
            yield return new WaitForSeconds(2);
            enter_invisible();
            yield return new WaitForSeconds(2);
        }
    }





    // Animation Events:
    private void handle_animation_events(string x) => animation_events[x]();
    private void create_animation_events(){
        animation_events = new Dictionary<string, Action>(){
            {"play_footstep_sound",()=>sound.emit_footstep_audio()},
            {"play_croak_sound",()=>sound.emit_deep_croak_audio()},
            {"emit_footstep_particles",()=>particles.play("footstep")},
        };
    }





    // Linkage:
    protected void link_events(){
        link_health();
        link_movement();
        link_combat();
        link_animation_event();
    }

    protected void unlink(){
        unlink_health();
        unlink_movement();
        unlink_combat();
        unlink_animation_event();
    }

    void link_health() => health.damaged += sprite.play_damaged_flash;
    void unlink_health() => health.damaged -= sprite.play_damaged_flash;
    void link_movement() => get_movement().move_direction_changed += face_move_dir;
    void unlink_movement() => get_movement().move_direction_changed -= face_move_dir;
    void link_combat() => combat.attack_ended += idle_state;
    void unlink_combat() => combat.attack_ended -= idle_state;
    void link_animation_event() => animation_event.signal += handle_animation_events;
    void unlink_animation_event() => animation_event.signal -= handle_animation_events;
}
