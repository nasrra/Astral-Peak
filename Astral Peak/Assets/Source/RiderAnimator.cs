using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiderAnimator : AnimatorOverride{
    public static readonly int
        IDLE            = Animator.StringToHash("idle"),
        RUN             = Animator.StringToHash("run"),
        FRONT_STRIKE_1  = Animator.StringToHash("front_strike_1"),
        SWORD_SUMMON    = Animator.StringToHash("sword_summon");
    
    public void idle() => animator.Play(IDLE);
}
