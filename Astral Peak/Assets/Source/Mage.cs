using System;
using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using Entropek;
using Entropek.Collections;
using UnityEngine;

public class Mage : Boss<Movement>{
    public event Action switch_to_idle = null, switch_to_follow_and_attack;
    public event Action<BossAttack> switch_to_attack;
    [Header("Mage")]
    [SerializedDictionary("id","Transform")]
    [SerializeField] SerializedDictionary<string, Transform> teleport_points = new SerializedDictionary<string, Transform>();
    [SerializeField] GameObject surrounding_projectiles;
    [SerializeField] LineParticleEmittersHandler line_particle_emitters;
    [SerializeField] LineParticleEmitter teleport_trail;
    [SerializeField] SummoningCircleHandler summoning_circles;
    protected Dictionary<string, Action> phase_linker;
    protected Dictionary<string, Action> phase_unlinker;
    protected Dictionary<string, Action> exit_cutscene_states;
    
    // Base: 
    void Awake(){
        phase_linker = new Dictionary<string, Action>(){
            {"phase_1", link_phase_1},
            {"phase_2", link_phase_2},        
            {"cutscene_opening", link_cutscene_opening},
            {"cutscene_transition", link_cutscene_transition},
        };
        phase_unlinker = new Dictionary<string, Action>(){
            {"phase_1", unlink_phase_1},
            {"phase_2", unlink_phase_2},        
        };        
        exit_cutscene_states =  new Dictionary<string, Action>(){
            {"phase_1",()=>{
                idle(0);
            }},
            {"phase_2",()=>{
                teleport_phase_2(teleport_points["idle"].position);
                state.queue(()=>idle_fly_pattern());
                idle(6);                
            }}
        };
        gameObject.SetActive(false);
        //link_phase(current_phase);
    }
    void Start(){
        link_events();
        check_game_state();
        //switch_phase();
        ////idle(3);
    }
    void OnDestroy() => unlink_events();
    public void idle(float time){
        state.queue_and_start(
            time: time,
            start_action:switch_to_idle
        );
    }
    protected override void attack(BossAttack attack) => switch_to_attack?.Invoke(attack);
    protected override void enter_cutscene_state(){
        stop_all();
        switch_to_idle();
    } 
    protected override void exit_cutscene_state(){
        exit_cutscene_states[current_phase]();
    } 
    protected override Dictionary<string,Action> get_phase_linker() => phase_linker;
    protected override Dictionary<string,Action> get_phase_unlinker() => phase_unlinker;
    protected override void stop_all(){
        summoning_circles.off();
        stop_staff_lightning();
        disable_surrounding_projectiles();
        base.stop_all();
    }




    // Camera
    public void signature_adjust(){
        CameraController.instance.lerp_offset(x:null, y:6f, time:2);
        CameraController.instance.lerp_zoom(size: 16, time: 2);
        CameraController.instance.lerp_regulators(_x_bounds: Vector2.zero, _y_bounds: null, time: 2);
    }
    public void signature_shake() => CameraController.instance.shake_camera(amount: .5f, time: 7.30f, lock_shake: true);
    public void signature_reset(){
        CameraController.instance.reset_offset(2);
        CameraController.instance.reset_zoom(2);
        CameraController.instance.reset_regulators(2);
    }
    public void yell_camera_shake() => CameraController.instance.shake_camera(time: 3, amount: 0.75f, lock_shake: false);





    //phase 1.
    private void idle_phase_1(){
        no_state();
        state.stop();
        animator.Rebind();
        animator.Play("MageIdle",0,0);

    }
    private void attack_phase_1(BossAttack attack){
        movement.halt();
        combat.halt();
        state.queue_and_start(
            time: animator.get_clip_length(attack.animation_id),
            start_action:()=>animator.Play(attack.animation_id,0,0));
    }
    public void teleport_phase_1(){
        float offset = UnityEngine.Random.Range(8,17);
        Vector3 left_pos = new Vector3(target.position.x - offset, -8.5f,0);
        Vector3 right_pos = new Vector3(target.position.x + offset, -8.5f,0);
        Vector3 previous_pos = teleport_trail.transform.position;
        if(UnityEngine.Random.Range(0,2) == 0)
            transform.position = check_left_teleport(left_pos)? left_pos : right_pos;
        else
            transform.position = check_right_teleport(right_pos)? right_pos : left_pos;
        teleport_trail.emit_once(teleport_trail.transform.position, previous_pos);
        animator.Play("Mage1ExitTel",0,0);
    }
    private bool check_left_teleport(Vector3 pos){return pos.x > combat.left_arena_bound.position.x + 1;}
    private bool check_right_teleport(Vector3 pos){return pos.x < combat.right_arena_bound.position.x - 1;}
    private void set_hollow_target(GameObject x){
        Hollow hollow = x.GetComponent<Hollow>();
        hollow.set_target(target);
        hollow.summon_state();
    }
    void move_direction_changed(Vector2 direction){
        if(combat.is_attacking == true)
            return;
        if(direction == Vector2.left || direction == Vector2.right)
            animator.Play("MageWalk",0,0);
        else if(direction == Vector2.zero)
            animator.Play("MageIdle",0,0);
    }





    // phase 2.
    public void enable_surrounding_projectiles(){
        surrounding_projectiles.SetActive(true);
        surrounding_projectiles.GetComponent<ObjectOrbiter>().start_behaviour();
    }
    public void disable_surrounding_projectiles(){
        surrounding_projectiles.SetActive(false);
    }
    private void idle_phase_2(){
        animator.Rebind();
        animator.Play("MageHover",0,0);
    }   
    public void start_staff_lightning(){
        for(int i = 0; i < 3; i++)
            line_particle_emitters.start_emitting("staff_lightning_"+i);
    }
    public void stop_staff_lightning(){
        for(int i = 0; i < 3; i++)
            line_particle_emitters.stop_emitting("staff_lightning_"+i);
    }
    public void turn_on_wailing_stones() => summoning_circles.turn_on(); 
    public void turn_off_wailing_stones() => summoning_circles.turn_off();
    private void attack_phase_2(BossAttack attack){
        if(teleport_points.ContainsKey(attack.animation_id)){
            teleport_phase_2(teleport_points[attack.animation_id].position);
            state.queue(time: animator.get_clip_length(attack.animation_id), start_action: () => animator.Play(attack.animation_id));
            teleport_phase_2(teleport_points["idle"].position);
            state.queue(()=>idle_fly_pattern());
            state.queue_and_start(combat.attack_end);
        }
        else
            state.queue_and_start(
                time: animator.get_clip_length(attack.animation_id),
                start_action:()=>animator.Play(attack.animation_id));
    }
    public void teleport_phase_2(Vector3 pos){
        state.queue(
            time: animator.get_clip_length("Mage2EnterTel"),
            start_action: ()=>{
                movement.halt();
                animator.Play("Mage2EnterTel");
            }
        );
        state.queue(
            time: animator.get_clip_length("Mage2ExitTel"),
            start_action: () =>{
                teleport_trail.emit_once(transform.position,pos);
                transform.position = pos;
                animator.Play("Mage2ExitTel");                
            }
        );
    }
    private void idle_fly_pattern(){
        movement.halt();
        movement.figure_eight_state(x_factor:.133f, y_factor:.0665f, reverse: UnityEngine.Random.Range(0,2) == 0);
    }
    private void fly_and_attack_state(){
        animator.Play("MageHover");
        combat.chose_attack_state(target);
    }





    // death
    void teleport_death(Vector2 pos)=>
        StartCoroutine(Util.timer(
            time: 0.5f,
            start_action: ()=>{
                sprites.play_death_effect(0.5f);
                sound.play_non_diegetic_one_shot("mage_teleport");
            },
            time_out: ()=>{
                sprites.play_death_effect_reverse(0.5f);
                teleport_trail.emit_once(transform.position, pos);
                transform.position = pos;
                sound.play_non_diegetic_one_shot("boss_yell_non_diegetic");
            }
        ));

    protected override void death_start() => StartCoroutine(death_loop());
    IEnumerator death_loop(){
        no_state();
        enable_body_colliders(0); 
        stop_all();
        signature_adjust();
        teleport_death(new Vector2(0,0));
        animator.Play("MageDeath");
        particles.play_particle("yell");
        CameraController.instance.start_camera_shake(0.75f, true);
        base.death_start();
        yield return new WaitForSeconds(1.5f);
        int count = 0;
        while(count < 5){
            int x = UnityEngine.Random.Range(0,10) - 5;
            int y = UnityEngine.Random.Range(0,10) - 5;
            teleport_death(new Vector2(x,y));
            count++;
            yield return new WaitForSeconds(1.25f);
        }
        teleport_death(new Vector2(0,2));
        sprites.play_death_effect(4);
        yield return new WaitForSeconds(4);
        CameraController.instance.stop_camera_shake();
        particles.stop_particle("yell");
        yield return new WaitForSeconds(particles.get_particle("yell").main.startLifetime.constantMax  + 1);
        signature_reset();
        base.death_complete();
        Destroy(gameObject);
        yield break;
    }




    protected void play_weapon_tip_flash(){
        lighting.lerp_intensity("staff_tip", start: 1, end: 0, time: .5f);
    }
    public void phase_2_body_charged(){
        sprites.lerp_charged_amount(.35f, 5);
    }






    // phase link:

    private void link_phase_1(){
        Log.MethodCall();
        set_phase_data("phase_1");
        link_components();
        health.death += transition_phase;
        state = new StateQueue(this, follow_and_attack_state);
        switch_to_idle = idle_phase_1;
        switch_to_follow_and_attack = follow_and_attack_state;
        switch_to_attack = attack_phase_1;
    }
    private void unlink_phase_1(){
        Log.MethodCall();
        unlink_components();
        health.death -= transition_phase;
    }
    private void link_phase_2(){
        Log.MethodCall();
        set_phase_data("phase_2");
        link_components();
        unlink_movement();
        health.death += kill;
        state = new StateQueue(this, fly_and_attack_state);
        switch_to_idle = idle_phase_2;
        switch_to_follow_and_attack = fly_and_attack_state;
        switch_to_attack = attack_phase_2;
    }
    private void unlink_phase_2(){//
        Log.MethodCall();
        unlink_components();
    }
    private void link_cutscene_opening(){
        Log.MethodCall();
        switch_to_idle = idle_phase_1;
    }
    private void link_cutscene_transition(){
        Log.MethodCall();
        switch_to_idle = idle_phase_1;
    }




    // Linkage:
    protected void link_events(){
        link_game_manager();
    }
    protected void unlink_events(){
        unlink_game_manager();
        unlink_components();
        health.death -= transition_phase;
        health.death -= kill;
    }
    protected void link_components(){
        link_health();
        link_movement();
        link_combat();
        link_ranged();   
        link_particles();     
    }
    protected void unlink_components(){
        unlink_health();
        unlink_movement();
        unlink_combat();
        unlink_ranged();        
        unlink_particles();     
    }
    void link_health(){
        health.damaged += sprites.play_damaged_flash;
    }
    void unlink_health(){
        health.damaged -= sprites.play_damaged_flash;
    }
    protected override void link_movement(){
        movement.move_direction_changed += face_direction;
        movement.move_direction_changed += move_direction_changed;
        base.link_movement();
    }
    protected override void unlink_movement(){
        movement.move_direction_changed -= face_direction;
        movement.move_direction_changed -= move_direction_changed;
        base.unlink_movement();
    }
    protected override void link_combat(){
        combat.attack_ended += idle;
        combat.attack_chosen += attack;
        base.link_combat();
    }
    protected override void unlink_combat(){
        combat.attack_ended -= idle;
        combat.attack_chosen -= attack;
        base.unlink_combat();
    }
    void link_ranged(){
        for(int i = 1; i < 7; i++)
            ranged.get_holster("hollow_"+i).projectile_fired += set_hollow_target;
        ranged.get_holster("lightning_strike").set_fire_point(target);
    }
    void unlink_ranged(){
        for(int i = 1; i < 7; i++)
            ranged.get_holster("hollow_"+i).projectile_fired -= set_hollow_target;
    }
}
