using UnityEngine;

[System.Serializable]
public class PlayerAnimator : AnimatorOverride{
    public static readonly int 
        IDLE    = Animator.StringToHash("idle"),
        ATTACK  = Animator.StringToHash("attack"),
        RUN     = Animator.StringToHash("run"),
        GUARD   = Animator.StringToHash("guard"),
        FALL_START  = Animator.StringToHash("fall_start"),
        FALL_LOOP   = Animator.StringToHash("fall_loop"),
        LBOUNCE = Animator.StringToHash("light_bounce"),
        MBOUNCE = Animator.StringToHash("medium_bounce"),
        HBOUNCE = Animator.StringToHash("heavy_bounce"),
        NONE    = Animator.StringToHash("none");

    void Start() => state = IDLE;

    // main states that can be returned to.
    public void idle()      {if(state != FALL_START && state != FALL_LOOP) play(IDLE, true);}
    public void run()       {if(state != FALL_START && state != FALL_LOOP) play(RUN, true);}
    public void none()      => play(NONE, true);
    public void start_fall()      => play(FALL_START, true);
    public void loop_fall()       => play(FALL_LOOP, false); // key event in jump start animation.
    public void jump(){
        play(FALL_START, true);
        play_override(MBOUNCE);
    }
    
    // additive states that should not be returned to.
    public void attack()    => play(ATTACK, false); 
    
    // used for the override animation layer to return to the none state
    public void none_state() => a.Play(NONE, OVERRIDE);

    // override states
    public void light_bounce()  => play_override(LBOUNCE);
    public void medium_bounce() => play_override(MBOUNCE);
    public bool is_falling() => state == FALL_LOOP || state == FALL_START;
}