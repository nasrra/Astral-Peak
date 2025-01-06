using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using Deluz;
using Deluz.Collections;
using UnityEngine;

public class Mage : Boss<Movement>{
    public event Action switch_to_idle, switch_to_follow_and_attack;
    public event Action<BossAttack> switch_to_attack;
    [Header("Mage")]
    [SerializedDictionary("id","Transform")]
    [SerializeField] SerializedDictionary<string, Transform> teleport_points = new SerializedDictionary<string, Transform>();
    [SerializeField] SerializedDictionary<string, MovementData> movement_presets = new SerializedDictionary<string, MovementData>();
    [SerializeField] List<LineParticleEmitter> staff_lightning = new List<LineParticleEmitter>();
    [SerializeField] GameObject surrounding_projectiles;
    [SerializeField] LineParticleEmitter teleport_trail;
    [SerializeField] SummoningCircleHandler summoning_circles;
    
    // Base: 
    void Start(){
        create_phase_linkage();
        switch_phase();
        link_events();
        sound.set_functions(new MageSound(sound));
        check_game_state();
        idle(3);
    }
    void OnDestroy() => unlink_events();
    public void idle(float time){
        state.queue_and_start(
            time: time,
            start_action:switch_to_idle
        );
    }
    protected override void attack(BossAttack attack) => switch_to_attack?.Invoke(attack);
    public override void enter_cutscene_state(){
        movement.clear_state();
        combat.no_state();
        state.stop();
        switch_to_idle();
    } 
    public override void exit_cutscene_state(){
        switch_phase();
        idle(3);
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
        animator.Play("MageIdle");
    }
    private void attack_phase_1(BossAttack attack){
        movement.halt();
        combat.no_state();
        state.queue_and_start(
            time: animator.get_clip_length(attack.animation_id),
            start_action:()=>animator.Play(attack.animation_id));
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
        animator.Play("Mage1ExitTel");
    }
    private bool check_left_teleport(Vector3 pos){return pos.x > combat.left_arena_bound.position.x + 1;}
    private bool check_right_teleport(Vector3 pos){return pos.x < combat.right_arena_bound.position.x - 1;}
    private void set_hollow_target(GameObject x){
        Hollow hollow = x.GetComponent<Hollow>();
        hollow.set_target(target);
        hollow.on_start += hollow.summon_state;
    }
    void move_direction_changed(Vector2 direction){
        if(combat.is_attacking == true)
            return;
        if(direction == Vector2.left || direction == Vector2.right)
            animator.Play("MageWalk",0,0);
        else
            animator.Play("MageIdle",0,0);
    }





    // phase 2.
    public void enable_surrounding_projectiles() => surrounding_projectiles.SetActive(true);
    public void disable_surrounding_projectiles() => surrounding_projectiles.SetActive(false);
    private void idle_phase_2() => animator.Play("MageHover");
    public void start_staff_lightning(){
        foreach(LineParticleEmitter l in staff_lightning)
            l.start_emitting();
    }
    public void stop_staff_lightning(){
        foreach(LineParticleEmitter l in staff_lightning)
            l.stop_emitting();
    }
    public void turn_on_wailing_stones() => summoning_circles.turn_on(); 
    public void turn_off_wailing_stones() => summoning_circles.turn_off();
    private void attack_phase_2(BossAttack attack){
        if(teleport_points.ContainsKey(attack.animation_id)){
            movement.halt();
            state.queue_and_start(new List<StateQueueItem>{
                new(
                    _time: animator.get_clip_length("Mage2EnterTel"),
                    _start_action: () => animator.Play("Mage2EnterTel")
                ),
                new(
                    _time: animator.get_clip_length("Mage2ExitTel"),
                    _start_action: () => teleport_phase_2(teleport_points[attack.animation_id].position)
                ),
                new(
                    _time: animator.get_clip_length(attack.animation_id),
                    _start_action: () => animator.Play(attack.animation_id)
                ),
                new(
                    _time: animator.get_clip_length("Mage2EnterTel"),
                    _start_action: () => animator.Play("Mage2EnterTel")
                ),
                new(
                    _time: animator.get_clip_length("Mage2ExitTel"),
                    _start_action: () => set_fly_pattern()
                ),
                new(
                    _time: 0,
                    _start_action: () => combat.attack_end()
                )
            });
        }
        else
            state.queue_and_start(
                time: animator.get_clip_length(attack.animation_id),
                start_action:()=>animator.Play(attack.animation_id));
    }
    private void fly_and_attack_state(){
        animator.Play("MageHover");
        combat.chose_attack_state(target);
    }
    private void set_fly_pattern(){
        int x = UnityEngine.Random.Range(0,2);
        animator.Play("Mage2ExitTel");
        movement.halt();
        teleport_phase_2(x==0?teleport_points["figure_eight"].position : teleport_points["figure_eight_reversed"].position);
        movement.figure_eight_state(reverse:x==0?false:true);
    }
    public void teleport_phase_2(Vector3 pos){
        teleport_trail.emit_once(transform.position,pos);
        transform.position = pos;
        animator.Play("Mage2ExitTel");
    }



    // VFX Calls
    public void signature_2_lighting(){
        SceneLighting.instance.set_intensity("global", 2f);
        SceneLighting.instance.reset_lighting("global", .2f);
    }
    protected void play_weapon_flash(){
        sprites.play_charged_flash("staff");
        lighting.lerp_intensity("staff", start: 1, end: 0, time: 1); 
        sound.play_sound("ping");
    }
    protected void play_weapon_tip_flash(){
        lighting.lerp_intensity("staff_tip", start: 1, end: 0, time: .5f);
    }




    // Linkage:
    protected void link_events(){
        link_game_manager();
    }
    protected void unlink_events(){
        unlink_game_manager();
        unlink_health();
        unlink_movement();
        unlink_combat();
        unlink_ranged();
    }
    protected override void create_phase_linkage(){
        phase_linker = new Dictionary<int, Action>(){
            {1, ()=>link_phase_1()},
            {2, ()=>link_phase_2()},        
        };
        phase_unlinker = new Dictionary<int, Action>(){
            {1, ()=>unlink_phase_1()},
            {2, ()=>unlink_phase_2()},        
        };    
    }
    void link_phase_1(){
        movement.set_data(movement_presets["phase_1"]);      
        link_health();
        health.death += next_phase;
        link_movement();
        link_combat();
        link_ranged();
        state = new StateQueue(this, follow_and_attack_state);
        switch_to_idle = idle_phase_1;
        switch_to_follow_and_attack = follow_and_attack_state;
        switch_to_attack = attack_phase_1;
    }
    void unlink_phase_1(){
        sound.stop_all_loops();
        particles.stop_all_particles();
        unlink_health();
        health.death -= next_phase;
        unlink_movement();
        unlink_combat();
        unlink_ranged();
    }
    void link_phase_2(){
        movement.set_data(movement_presets["phase_2"]);
        link_health();
        health.set_max_life(40);
        health.set_current_life(40);
        link_combat();
        link_ranged();
        set_fly_pattern();
        state = new StateQueue(this, fly_and_attack_state);
        switch_to_idle = idle_phase_2;
        switch_to_follow_and_attack = fly_and_attack_state;
        switch_to_attack = attack_phase_2;
    }
    void unlink_phase_2(){
        unlink_health();
        unlink_combat();
        unlink_ranged();
    }
    void link_health() => health.damaged += sprites.play_damaged_flash;
    void unlink_health() => health.damaged -= sprites.play_damaged_flash;
    void link_movement(){
        movement.move_direction_changed += face_direction;
        movement.move_direction_changed += move_direction_changed;
    }
    void unlink_movement(){
        movement.move_direction_changed -= face_direction;
        movement.move_direction_changed -= move_direction_changed;
    }
    void link_combat(){
        combat.attack_ended += idle;
        combat.attack_chosen += attack;
    }
    void unlink_combat(){
        combat.attack_ended -= idle;
        combat.attack_chosen -= attack;
    }
    void link_ranged(){
        for(int i = 1; i < 7; i++)
            ranged.get_holster("hollow_"+i).projectile_fired += set_hollow_target;
        ranged.get_holster("lightning_strike").set_fire_point(Player.instance.transform);
    }
    void unlink_ranged(){
        for(int i = 1; i < 7; i++)
            ranged.get_holster("hollow_"+i).projectile_fired -= set_hollow_target;
    }
}
