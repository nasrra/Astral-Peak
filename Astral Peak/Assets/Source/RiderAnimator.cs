using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiderAnimator : AnimatorOverride{
    public static readonly int
        IDLE            = Animator.StringToHash("idle"),
        RUN             = Animator.StringToHash("run"),
        SIGNATURE       = Animator.StringToHash("signature"),
        FRONT_STRIKE    = Animator.StringToHash("front_strike"),
        JUMP_N_DASH     = Animator.StringToHash("jump_n_dash"),
        BACK_SHOT       = Animator.StringToHash("back_shot"),
        RUN_N_GUN       = Animator.StringToHash("run_n_gun"),
        ROUND_SHOT      = Animator.StringToHash("round_shot"),
        YELL            = Animator.StringToHash("yell"),
        WHISTLE         = Animator.StringToHash("whistle");
    
    public void idle() => animator.Play(IDLE);
}
