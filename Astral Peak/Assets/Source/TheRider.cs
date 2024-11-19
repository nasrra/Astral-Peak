using System.Collections;
using UnityEngine;

public class TheRider : Boss<RiderMovement>{
    public static TheRider instance;

    [SerializeField] RiderParticlesHandler particles;
    [SerializeField] RiderRangedCombat ranged;
    [SerializeField] RiderAudio sound;
    public void yell_camera_shake() => CameraController.instance.shake_camera(3,0.75f);

    void OnEnable(){
        instance = this;
        state_switch(idle(1));
        link_events();
    }

    void OnDisable() => unlink_events();

    public override void enter_cutscene_state() => state_switch(lock_idle());
    public override void exit_cutscene_state() => state_switch(idle(1));

    public void switch_to_idle(float x) => state_switch(idle(x));
    IEnumerator idle(float x){
        animator.Play(RiderAnimator.IDLE);
        yield return new WaitForSeconds(x);
        state_switch(follow());
        yield break;
    }

    public void cutscene_yell_state() => state_switch(cutscene_yell());
    IEnumerator cutscene_yell(){
        animator.Play(RiderAnimator.YELL); 
        yield return new WaitForSeconds(3f);
        state_switch(lock_idle());
        yield break;        
    }

    public void cutscene_whistle_state() => state_switch(cutscene_whistle());
    IEnumerator cutscene_whistle(){
        animator.Play(RiderAnimator.WHISTLE);
        yield return new WaitForSeconds(2.1f);
        state_switch(lock_idle());
        yield break;
    }

    IEnumerator yell(){
        animator.Play(RiderAnimator.YELL); 
        yield return new WaitForSeconds(3);
        state_switch(idle(1));
        yield break;
    }

    IEnumerator lock_idle(){
        animator.Play(RiderAnimator.IDLE);
        yield break;
    }

    protected override IEnumerator follow(){
        animator.Play(RiderAnimator.RUN);
        state_switch(base.follow());
        yield break;
    }

    protected void link_events(){
        movement.move_direction_changed         += face_move_dir;
        health.death                            += kill;
        health.death                            += get_movement().StopAllCoroutines;
        combat.attack_ended                     += switch_to_idle;
        health.damaged                          += sprite.play_damaged_flash;
        flipped_left                            += particles.flip_left;
        flipped_right                           += particles.flip_right;
        ranged.arrow_fired                      += sound.emit_arrow_shot;
    }

    protected void unlink_events(){
        movement.move_direction_changed         -= face_move_dir;
        health.death                            -= kill;
        health.death                            -= get_movement().StopAllCoroutines;
        combat.attack_ended                     -= switch_to_idle;
        health.damaged                          -= sprite.play_damaged_flash;
        flipped_left                            -= particles.flip_left;
        flipped_right                           -= particles.flip_right;
        ranged.arrow_fired                      -= sound.emit_arrow_shot;
    }

}
