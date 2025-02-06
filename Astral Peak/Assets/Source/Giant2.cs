using UnityEngine;
using Entropek;
using System;
using Unity.VisualScripting;

public class Giant2 : Boss<Movement>{
    Coroutine idle_state;
    [Header("Giant2")]
    [SerializeField] FinalBossRoomGroundHandler ground_handler;
    private int HEAD_LAYER = 0, LHAND_LAYER = 1, RHAND_LAYER = 2;

    void Awake(){
        sound.set_functions(new GiantSound(gameObject));
        link_events();
    }
    void Start(){
        set_phase_data("phase_1");
        //switch_phase();
        //idle(3);
    }

    private void idle(){
        animator.Play("Giant2Idle", HEAD_LAYER);
        animator.Play("Giant2Idle", LHAND_LAYER);
        animator.Play("Giant2Idle", RHAND_LAYER);
        no_state();
    }

    private void stop_idle_loop(){
        if(idle_state != null)
            StopCoroutine(idle_state);
    }

    private void idle(float time){
        stop_idle_loop();
        idle_state = StartCoroutine(Util.timer(
            time,
            start_action:()=>idle(),
            time_out:()=>follow_and_attack_state()
        ));
    }

    int get_current_ground_piece(){
        int x = -1;
        Collider2D other = Physics2D.OverlapCircle(transform.position, .5f, LayersManager.BITWISE_GROUND);
        if(other != null)
            Int32.TryParse(other.name, out x);
        return x;
    }

    protected void link_events(){
        link_game_manager();
        health.death                            += kill;
        health.death                            += movement.StopAllCoroutines;
        combat.attack_chosen                    += attack;
        combat.attack_ended                     += idle;
        health.damaged                          += sprites.play_damaged_flash;
        link_combat();
        link_movement();
        link_particles();
    }

    protected void unlink_events(){
        unlink_game_manager();
        health.death                            -= kill;
        health.death                            -= movement.StopAllCoroutines;
        combat.attack_chosen                    -= attack;
        combat.attack_ended                     -= idle;
        health.damaged                          -= sprites.play_damaged_flash;
        unlink_particles();
        unlink_combat();
        unlink_movement();
    }
}
