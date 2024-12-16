using System.Collections;
using UnityEngine;

public class TheMage : Boss<Movement>{

    [Header("The Mage")]
    [SerializeField] ParticlesHandler particles;
    [SerializeField] MageAnimator animator;



    // Base: 
    void Awake(){
        link_events();
    }

    void Start(){
        state_switch(idle(2));
        StartCoroutine(test());
    }

    void OnDestroy(){
        unlink_health();
    }





    //
    IEnumerator lock_idle(){
        animator.Play("idle");
        yield break;
    }

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

    void enter_invisible(){
        animator.invisible(true);
    }

    void exit_invisible(){
        animator.invisible(false);
    }
    IEnumerator test(){
        while(true){
            exit_invisible();
            yield return new WaitForSeconds(2);
            enter_invisible();
            yield return new WaitForSeconds(2);
        }
    }






    // Linkage:
    protected void link_events(){
        link_health();
        link_movement();
    }

    protected void unlink(){
        unlink_health();
        unlink_movement();
    }

    void link_health(){
        health.damaged += sprite.play_damaged_flash;
    }

    void unlink_health(){
        health.damaged -= sprite.play_damaged_flash;
    }

    void link_movement(){
        get_movement().move_direction_changed += face_move_dir;
    }

    void unlink_movement(){
        get_movement().move_direction_changed -= face_move_dir;
    }
}
