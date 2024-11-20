using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheCavalry : Boss<CavalryMovement>{
    public static TheCavalry instance;

    // Start is called before the first frame update
    [SerializeField] CavalryParticlesHandler particles;
    [SerializeField] CavalryRangedCombatHandler ranged;
    [SerializeField] CavalryMeleeCombatHandler melee;
    [SerializeField] CavalryAudio sound;
    [SerializeField] FetchSword fetch_sword;
    [SerializeField] List<Collider2D> body_colliders;
    [SerializeField] float
        follow_speed,
        follow_fsword_speed;

    void OnEnable(){
        instance = this;
        state_switch(idle(1));
        link_events();
    } 

    void OnDisable() {
        unlink_events();
        StopAllCoroutines();
    }

    public void switch_to_move_to_fetch_sword() => state_switch(follow_fetch_sword());
    public void switch_to_idle_no_sword() => state_switch(idle_no_sword());
    public void switch_to_idle(float x) => state_switch(idle(x));

    // states: 
    public override void enter_cutscene_state() => state_switch(lock_idle());
    public override void exit_cutscene_state() => state_switch(idle(1));

    protected override void kill(){
        base.kill();
        state_switch(death_state());
    }

    AudioSource source;
    IEnumerator death_state(){
        foreach(Collider2D c in body_colliders)
            c.enabled = false;
        animator.Play(CavalryAnimator.DEATH);
        sprite.play_death_effect(2.25f);
        disable_components();
        movement.zero_velocity(); // stop velocity in case the boss is dashing.
        particles.stop_all_particles();
        yield return new WaitForSeconds(6);
        AudioManager.stop_music();
        UiManager.instance.play_enemy_vanquished();
        AudioClipHandler.play(UnityHook.instance, SoundID.WOODEN_PING, out source);
        gameObject.SetActive(false);
        yield break;
    }

    void disable_components(){
        particles.StopAllCoroutines();
        particles.enabled = false;
        ranged.StopAllCoroutines();
        ranged.enabled = false;
        movement.StopAllCoroutines();
        movement.enabled = false;
        melee.StopAllCoroutines();
        melee.enabled = false;
        combat.StopAllCoroutines();
        combat.enabled = false;
    }

    IEnumerator idle(float x){ 
        animator.Play(CavalryAnimator.IDLE);
        yield return new WaitForSeconds(x);
        state_switch(follow());
        yield break;
    }

    IEnumerator lock_idle(){
        animator.Play(CavalryAnimator.IDLE);
        yield break;
    }

    IEnumerator idle_no_sword(){
        animator.Play(CavalryAnimator.NO_SWORD_IDLE);
        yield break;
    }

    IEnumerator pickup_sword(){
        animator.Play(CavalryAnimator.PICKUP_SWORD);
        Destroy(fetch_sword.gameObject);
        target = Player.player.transform;
        yield return new WaitForSeconds(1);
        state_switch(follow());
        yield break;
    }

    void fetch_sword_landed(){
        animator.Play(CavalryAnimator.WHISTLE);
    }

    protected override IEnumerator follow(){
        animator.Play(CavalryAnimator.RUN);
        movement.set_speed(follow_speed);
        state_switch(base.follow());
        yield break;
    }

    IEnumerator follow_fetch_sword(){
        target = fetch_sword.transform;
        movement.set_speed(follow_fsword_speed);
        animator.Play(CavalryAnimator.NO_SWORD_RUN);
        while(true){
            
            float dist = dist_to_target();
            
            // if we are not moving right, move right.
            if(dist < 0 && movement.get_move_direction() != new Vector2(1,0)){
                movement.stop();
                movement.move_right(true);
            }
            // if we are not moving left, move left.
            if(dist > 0 && movement.get_move_direction() != new Vector2(-1,0)){
                movement.stop();
                movement.move_left(true);
            }

            if(Mathf.Abs(dist) <= 0.5f){
                state_switch(pickup_sword());
                yield break;
            }
            yield return null;
        }
    }

    void link_fetch_sword(GameObject sword){
        fetch_sword = sword.GetComponent<FetchSword>();
        fetch_sword.landed += fetch_sword_landed;
        fetch_sword.landed += unlink_fetch_sword;
    }
    void unlink_fetch_sword() => fetch_sword.landed -= fetch_sword_landed;

    protected void link_events(){
        health.death                            += kill;
        health.death                            += get_movement().StopAllCoroutines;
        combat.attack_ended                     += switch_to_idle;
        ranged.fetch_sword_fired                += link_fetch_sword;
        ranged.arrow_fired                      += sound.emit_bow_shot;
        get_movement().move_direction_changed   += face_move_dir;
        flipped_left                            += particles.flip_left;
        flipped_right                           += particles.flip_right;
        health.damaged                          += sprite.play_damaged_flash;
    }

    protected void unlink_events(){
        health.death                            -= kill;
        health.death                            -= get_movement().StopAllCoroutines;
        combat.attack_ended                     -= switch_to_idle;
        ranged.fetch_sword_fired                -= link_fetch_sword;
        ranged.arrow_fired                      -= sound.emit_bow_shot;
        get_movement().move_direction_changed   -= face_move_dir;
        flipped_left                            -= particles.flip_left;
        flipped_right                           -= particles.flip_right;
        health.damaged                          -= sprite.play_damaged_flash;
    }
}
