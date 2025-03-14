using UnityEngine;

[System.Serializable]
public class PlayerAnimator : CharacterAnimatorOverride{
    public readonly int 
        IDLE        = Animator.StringToHash("idle"),
        SIDE_ATTACK = Animator.StringToHash("side_attack"),
        UP_ATTACK   = Animator.StringToHash("up_attack"),
        RUN         = Animator.StringToHash("run"),
        GUARD       = Animator.StringToHash("guard"),
        FALL_START  = Animator.StringToHash("fall_start"),
        FALL_LOOP   = Animator.StringToHash("fall_loop"),
        LBOUNCE     = Animator.StringToHash("light_bounce"),
        MBOUNCE     = Animator.StringToHash("medium_bounce"),
        HBOUNCE     = Animator.StringToHash("heavy_bounce"),
        NONE        = Animator.StringToHash("none"),
        DEATH       = Animator.StringToHash("death"),
        UP_TOGGLE   = Animator.StringToHash("up_toggle");

    void Start() => state = IDLE;
    

    // main states that can be returned to.
    public void force_idle(){
        play_instant(IDLE, true);
    }
    public void idle()          {if(state != FALL_START && state != FALL_LOOP) play(IDLE, true);}
    public void run()           {if(state != FALL_START && state != FALL_LOOP) play(RUN, true);}
    public void none()          => play(NONE, true);
    public void start_fall()    {if(is_falling() == false)play(FALL_START, true);}
    public void loop_fall()     => play(FALL_LOOP, false); // key event in jump start animation.
    public void death(){
        unlock_layers();
        play_instant(DEATH, true);
    }
    public void jump(){
        play(FALL_START, true);
        play_bounce_override(MBOUNCE);
    }


    // additive states that should not be returned to.
    public void side_attack(){
       play_instant(SIDE_ATTACK, false);
    } 
    public void up_attack(){
        play_instant(UP_ATTACK, false);
    }      

    //BOUNCE OVERRIDE STATES:
    // used for the override animation layer to return to the none state
    public void stop_bounce() => animator.Play(NONE, BOUNCE_OVERRIDE);
    public void light_bounce()  => play_bounce_override(LBOUNCE);
    public void medium_bounce() => play_bounce_override(MBOUNCE);

    public void up_toggle() => play_body_override(UP_TOGGLE);
    public void stop_up_toggle() => animator.Play(NONE, BODY_OVERRIDE);

    public bool is_falling() => state == FALL_LOOP || state == FALL_START;
}