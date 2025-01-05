using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using Deluz.Collections;
using UnityEngine;

public class Mage : Boss<Movement>{
    public event Action switch_to_idle, switch_to_follow_and_attack;
    public event Action<BossAttack> switch_to_attack;
    [Header("Mage")]
    [SerializeField] List<Animator> summoning_circles = new List<Animator>();
    [SerializedDictionary("id","Transform")]
    [SerializeField] SerializedDictionary<string, Transform> teleport_points= new SerializedDictionary<string, Transform>();
    [SerializeField] List<LineParticleEmitter> staff_lightning = new List<LineParticleEmitter>();
    [SerializeField] GameObject surrounding_projectiles;
    [SerializeField] LineParticleEmitter teleport_trail;
    StateQueue state  = new StateQueue(null, null);

    // Base: 
    void Start(){
        link_events();
        create_phase_linkage();
        sound.set_functions(new MageSound(sound));
        check_game_state();
        
        //  debug purposes.
        select_phase(phase);
        state.state_switch();
    }
    void OnDestroy() => unlink_events();
    public void idle(float time){
        state.queue(
            time: time,
            start_action:()=>switch_to_idle()
        );
        state.state_switch();
    }
    protected override void attack(BossAttack attack) => switch_to_attack?.Invoke(attack);
    protected override void entered_game_state(GameState state){
        if(state == GameState.CUTSCENE)
            enter_cutscene_state();
    }
    protected override void exited_game_state(GameState state){
        if(state == GameState.CUTSCENE)
            exit_cutscene_state();
    }
    public override void enter_cutscene_state(){
        no_state();
        state = new StateQueue(this, ()=>switch_to_idle.Invoke());
    } 
    public override void exit_cutscene_state(){
        select_phase(phase);
        state.state_switch();
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
        animator.Play("MageIdle");
    }
    private void attack_phase_1(BossAttack attack){
        state.queue(
                time: animator.get_clip_length(attack.animation_id),
                start_action:()=>animator.Play(attack.animation_id));
        state.state_switch();
    }
    private void movement_phase_1(){
        movement.set_gravity(1);
        movement.set_deceleration(.85f);
        movement.set_speed(3);
    }
    public void teleport_phase_1(){
        float random = UnityEngine.Random.Range(0,2);
        float offset = UnityEngine.Random.Range(8,17);
        Vector3 left_pos = new Vector3(target.position.x - offset, -8.5f,0);
        Vector3 right_pos = new Vector3(target.position.x + offset, -8.5f,0);
        Vector3 previous_pos = teleport_trail.transform.position;
        if(random == 0)
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
    public void turn_on_wailing_stones(){
        foreach(Animator circle in summoning_circles)
            circle.Play("turn_on");
    }
    public void turn_off_wailing_stones(){
        foreach(Animator circle in summoning_circles)
            circle.Play("turn_off");
    }
    private void attack_phase_2(BossAttack attack){
        if(teleport_points.ContainsKey(attack.animation_id)){
            Vector3 previous_pos = Vector3.zero;
            state.queue(
                time: animator.get_clip_length("Mage2EnterTel"),
                start_action:() => {
                    movement.no_state();
                    movement.zero_velocity();
                    previous_pos = teleport_trail.transform.position;
                    animator.Play("Mage2EnterTel");
                });
            state.queue(
                time: animator.get_clip_length("Mage2ExitTel"), 
                start_action:()=>{
                    teleport_phase_2(teleport_points[attack.animation_id].position);
                    teleport_trail.emit_once(teleport_trail.transform.position, previous_pos);
                });
            state.queue(
                time: animator.get_clip_length(attack.animation_id),
                start_action:()=>animator.Play(attack.animation_id));
            state.queue(
                time: animator.get_clip_length("Mage2EnterTel"),
                start_action:()=>{
                    previous_pos = teleport_trail.transform.position;
                    animator.Play("Mage2EnterTel");
                });
            state.queue(
                time: animator.get_clip_length("Mage2ExitTel"), 
                start_action:()=>{
                    set_fly_pattern();
                    teleport_trail.emit_once(teleport_trail.transform.position, previous_pos);
                });
            state.queue(
                time: 0,
                start_action:()=>combat.attack_end()
            );
        }
        else
            state.queue(
                time: animator.get_clip_length(attack.animation_id),
                start_action:()=>animator.Play(attack.animation_id));
        state.state_switch();
    }
    private void movement_phase_2(){
        movement.set_gravity(0);
        movement.set_deceleration(1f);
        movement.set_speed(6);
    }
    public void movement_sped_up_phase_2(){
        movement.set_gravity(0);
        movement.set_deceleration(1f);
        movement.set_speed(12);
    }
    private void fly_and_attack_state(){
        animator.Play("MageHover");
        combat.chose_attack_state(target);
    }
    private void set_fly_pattern(){
        int x = UnityEngine.Random.Range(0,2);
        animator.Play("Mage2ExitTel");
        movement.no_state();
        switch(x){
            case 0:
                transform.position = teleport_points["figure_eight"].position;
                movement.figure_eight_state(reverse:false);                
                break;
            case 1:
                transform.position = teleport_points["figure_eight_reversed"].position;
                movement.figure_eight_state(reverse:true);
                break;
        }
    }
    public void teleport_phase_2(Vector3 pos){
        transform.position = pos;
        animator.Play("Mage2ExitTel");
    }
    public void spawn_lightning_strike(){
        RaycastHit2D hit;
        Transform player = Player.instance.transform;
        hit = Physics2D.Raycast(player.position, Vector2.down, Mathf.Infinity, LayersManager.BITWISE_GROUND);
        if(hit==true)
            ranged.fire_projectile("lightning_strike",hit.point);
    }





    // VFX Calls
    public void signature_2_lighting(){
        SceneLighting.instance.lerp_intensity(
            id:"global",
            end:2f,
            time:.1f,
            callback:()=>SceneLighting.instance.reset_lighting(
                _id: "global",
                _time:.1f));
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
        link_phase_select();
        link_game_manager();
    }
    protected void unlink_events(){
        unlink_phase_select(); 
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
        movement_phase_1();
        link_health();
        link_movement();
        link_combat();
        link_ranged();
        state = new StateQueue(this, ()=>follow_and_attack_state());
        switch_to_idle = idle_phase_1;
        switch_to_follow_and_attack = follow_and_attack_state;
        switch_to_attack = attack_phase_1;
    }
    void unlink_phase_1(){
        unlink_health();
        unlink_movement();
        unlink_combat();
        unlink_ranged();
        state.clear();
    }
    void link_phase_2(){
        movement_phase_2();
        link_health();
        link_combat();
        link_ranged();
        set_fly_pattern();
        state = new StateQueue(this, ()=>fly_and_attack_state());
        switch_to_idle = idle_phase_2;
        switch_to_follow_and_attack = fly_and_attack_state;
        switch_to_attack = attack_phase_2;
    }
    void unlink_phase_2(){
        movement_phase_2();
        unlink_health();
        unlink_combat();
        unlink_ranged();
        set_fly_pattern();
        state.clear();
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
        ranged.get_holster("hollow_1").projectile_fired += set_hollow_target;
        ranged.get_holster("hollow_2").projectile_fired += set_hollow_target;
        ranged.get_holster("hollow_3").projectile_fired += set_hollow_target;
        ranged.get_holster("hollow_4").projectile_fired += set_hollow_target;
        ranged.get_holster("hollow_5").projectile_fired += set_hollow_target;
        ranged.get_holster("hollow_6").projectile_fired += set_hollow_target;
    }
    void unlink_ranged(){
        ranged.get_holster("hollow_1").projectile_fired -= set_hollow_target;
        ranged.get_holster("hollow_2").projectile_fired -= set_hollow_target;
        ranged.get_holster("hollow_3").projectile_fired -= set_hollow_target;
        ranged.get_holster("hollow_4").projectile_fired -= set_hollow_target;
        ranged.get_holster("hollow_5").projectile_fired -= set_hollow_target;
        ranged.get_holster("hollow_6").projectile_fired -= set_hollow_target;
    }
    void link_game_manager(){
        GameManager.entered_game_state += entered_game_state;
        GameManager.exited_game_state  += exited_game_state;
    }
    void unlink_game_manager(){
        GameManager.entered_game_state -= entered_game_state;
        GameManager.exited_game_state  -= exited_game_state;        
    }
}
