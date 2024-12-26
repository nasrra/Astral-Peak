using System.Collections;
using System.Collections.Generic;
using Deluz;
using UnityEngine;

public class TheMage : Boss<Movement>{
    [Header("Mage")]
    [SerializeField] int phase = 1;
    [SerializeField] List<Animator> summoning_circles = new List<Animator>();

    // Base: 
    void Awake(){
        link_events();
        sound.set_functions(new MageSound(sound));
    }
    void Start() => state_switch(idle(2));
    void OnDestroy() => unlink_events();




    public void wailing_stone_attack(){
        StartCoroutine(Util.timer(6, time_out: () =>{
            foreach(Animator circle in summoning_circles)
                circle.Play("loop");
        }));
        StartCoroutine(Util.timer(7.5f, time_out: () =>{
            for(int i = 0; i < summoning_circles.Count; i++)
                ranged.fire_projectile("stone_"+(i+1));
        }));
    }

    // states:
    IEnumerator lock_idle(){
        animator.Play("idle");
        yield break;
    }

    void idle_state(float x) => state_switch(idle(x));
    IEnumerator idle(float x){
        animator.Play("idle");
        yield return new WaitForSeconds(x);
        if(phase == 1)
            state_switch(follow_and_attack());
        else
            state_switch(fly_pattern());
        yield break;
    }

    protected IEnumerator fly_pattern(){
        float sin_x = 0;
        float sin_y = 0;
        movement.set_gravity(0);
        animator.Play("hover");
        while(true){
            sin_x = Mathf.Sin(Time.time * Time.deltaTime * 30);
            sin_y = Mathf.Sin(Time.time * Time.deltaTime * 60);
            movement.set_move_direction(new Vector2(sin_x, sin_y));
            yield return new WaitForFixedUpdate();
        }
    }

    protected override IEnumerator follow_and_attack(){
        animator.Play("walk");
        return base.follow_and_attack();
    }

    public void follow_only_state() => state_switch(base.follow_only());
    private void teleport(){
        float random = Random.Range(0,2);
        float offset = Random.Range(8,17);
        Vector3 left_pos = new Vector3(target.position.x - offset, -8.5f,0);
        Vector3 right_pos = new Vector3(target.position.x + offset, -8.5f,0);
        if(random == 0)
            transform.position = check_left_teleport(left_pos)? left_pos : right_pos;
        else
            transform.position = check_right_teleport(right_pos)? right_pos : left_pos;
        animator.Play("exit_teleport");
    }

    // used for when hollows are summoned.
    private void set_hollow_target(GameObject x){
        Hollow hollow = x.GetComponent<Hollow>();
        hollow.set_target(target);
        hollow.animator.Play("summon");
        hollow.on_start += hollow.summon_state;
    }

    private bool check_left_teleport(Vector3 pos){return pos.x > combat.left_arena_bound.position.x + 1;}
    private bool check_right_teleport(Vector3 pos){return pos.x < combat.right_arena_bound.position.x - 1;}

    // Linkage:
    protected void link_events(){
        link_health();
        if(phase == 1)
            link_movement();
        link_combat();
        link_ranged();
    }

    protected void unlink_events(){
        unlink_health();
        if(phase == 1)
            unlink_movement();
        unlink_combat();
        unlink_ranged();
    }

    void link_health() => health.damaged += sprite.play_damaged_flash;
    void unlink_health() => health.damaged -= sprite.play_damaged_flash;
    void link_movement() => get_movement().move_direction_changed += face_move_dir;
    void unlink_movement() => get_movement().move_direction_changed -= face_move_dir;
    void link_combat() => combat.attack_ended += idle_state;
    void unlink_combat() => combat.attack_ended -= idle_state;
    void link_ranged(){
        ranged.get_holster("hollow_1").projectile_fired += set_hollow_target;
        ranged.get_holster("hollow_2").projectile_fired += set_hollow_target;
    }
    void unlink_ranged(){
        ranged.get_holster("hollow_1").projectile_fired -= set_hollow_target;
        ranged.get_holster("hollow_2").projectile_fired -= set_hollow_target;
    }
}
