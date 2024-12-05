using DocumentFormat.OpenXml.Drawing;
using UnityEngine;

public class HollowAnimator : AnimatorOverride{
    [SerializeField] private Animator a;
    public static readonly int
        IDLE        = Animator.StringToHash("idle"),
        WALK        = Animator.StringToHash("walk"),
        ATTACK      = Animator.StringToHash("attack"),
        STUNNED     = Animator.StringToHash("stunned"),
        YELL        = Animator.StringToHash("yell"),
        RUN         = Animator.StringToHash("run");
}