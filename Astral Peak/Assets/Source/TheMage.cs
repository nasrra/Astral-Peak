using System;
using System.Collections.Generic;
using Deluz;
using DocumentFormat.OpenXml.Drawing;
using UnityEngine;

public class TheMage : Boss<Movement>{
    public event Action switch_to_idle, switch_to_follow_and_attack;
    public event Action<BossAttack> switch_to_attack;
    [Header("Mage")]
    [SerializeField] List<Animator> summoning_circles = new List<Animator>();
    Dictionary<int, Action> phase_linker;

    // Base: 
    void Awake(){
        link_events();
        create_phase_linker();
        select_phase(phase);
        sound.set_functions(new MageSound(sound));
        idle(2);
    }
    void OnDestroy() => unlink_events();




    public void wailing_stone_attack(){
        foreach(Animator circle in summoning_circles)
            circle.Play("loop");
        StartCoroutine(Util.timer(1.5f, time_out: () =>{
            for(int i = 0; i < summoning_circles.Count; i++)
                ranged.fire_projectile("stone_"+(i+1));
        }));
    }

    // states:
    private void idle(float time) =>
        StartCoroutine(Util.timer(
            time,
            start_action:()=>switch_to_idle(),
            time_out:()=>switch_to_follow_and_attack()
        ));

    private void idle_phase_1(){
        no_state();
        animator.Play("idle");
    }

    private void idle_phase_2(){
        animator.Play("hover");
    }

    protected override void attack(BossAttack attack) => switch_to_attack?.Invoke(attack);
    private void attack_phase_1(BossAttack attack) => base.attack(attack);
    private void attack_phase_2(BossAttack attack) => animator.Play(attack.animation_id);

    private void movement_phase_1(){
        movement.set_gravity(1);
        movement.set_deceleration(.85f);
        movement.set_speed(3);
    }

    private void movement_phase_2(){
        movement.set_gravity(0);
        movement.set_deceleration(1f);
        movement.set_speed(6);
    }

    private void fly_and_attack_state(){
        //movement.set_gravity(0);
        animator.Play("hover");
        combat.chose_attack_state(target);
    }

    // set a fly pattern at the end of every teleport.
    private void set_fly_pattern(){
        //Random.Range.
        movement.figure_eight_state();
    }

    public void teleport(){
        float random = UnityEngine.Random.Range(0,2);
        float offset = UnityEngine.Random.Range(8,17);
        Vector3 left_pos = new Vector3(target.position.x - offset, -8.5f,0);
        Vector3 right_pos = new Vector3(target.position.x + offset, -8.5f,0);
        if(random == 0)
            transform.position = check_left_teleport(left_pos)? left_pos : right_pos;
        else
            transform.position = check_right_teleport(right_pos)? right_pos : left_pos;
        animator.Play("exit_teleport");
    }
    private bool check_left_teleport(Vector3 pos){return pos.x > combat.left_arena_bound.position.x + 1;}
    private bool check_right_teleport(Vector3 pos){return pos.x < combat.right_arena_bound.position.x - 1;}


    // used for when hollows are summoned.
    private void set_hollow_target(GameObject x){
        Hollow hollow = x.GetComponent<Hollow>();
        hollow.set_target(target);
        hollow.animator.Play("summon");
        hollow.on_start += hollow.summon_state;
    }

    void move_direction_changed(Vector2 direction){
        if(combat.is_attacking == true)
            return;
        if(direction == Vector2.left || direction == Vector2.right)
            animator.Play("walk",0,0);
        else
            animator.Play("idle",0,0);
    }

    // Linkage:
    protected void link_events(){
        phase_selected += link_phase;
        phase_selected += combat.set_moveset;
    }
    protected void unlink_events(){
        unlink_health();
        unlink_movement();
        unlink_combat();
        unlink_ranged();
    }
        
    void link_phase(int phase) => phase_linker[phase]();
    void create_phase_linker(){
        unlink_events();
        phase_linker = new Dictionary<int, Action>(){
            {1, ()=>link_phase_1()},
            {2, ()=>link_phase_2()},        
        };
    }
    void link_phase_1(){
        movement_phase_1();
        link_health();
        link_movement();
        link_combat();
        link_ranged();
        switch_to_idle = idle_phase_1;
        switch_to_follow_and_attack = follow_and_attack_state;
        switch_to_attack = attack_phase_1;
    }
    void link_phase_2(){
        movement_phase_2();
        set_fly_pattern(); // here temporarily.
        link_health();
        link_combat();
        link_ranged();
        switch_to_idle = idle_phase_2;
        switch_to_follow_and_attack = fly_and_attack_state;
        switch_to_attack = attack_phase_2;
    }

    void link_health() => health.damaged += sprite.play_damaged_flash;
    void unlink_health() => health.damaged -= sprite.play_damaged_flash;
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
    }
    void unlink_ranged(){
        ranged.get_holster("hollow_1").projectile_fired -= set_hollow_target;
        ranged.get_holster("hollow_2").projectile_fired -= set_hollow_target;
    }
}
