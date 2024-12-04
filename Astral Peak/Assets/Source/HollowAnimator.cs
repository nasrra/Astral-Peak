using UnityEngine;

public class HollowAnimator : AnimatorOverride{
    [SerializeField] private Animator a;
    public static readonly int
        IDLE        = Animator.StringToHash("idle"),
        WALK        = Animator.StringToHash("walk"),
        ATTACK      = Animator.StringToHash("attack"),
        STUNNED     = Animator.StringToHash("stunned");


    public void idle()      => a.Play(IDLE);
    public void walk()      => a.Play(WALK);
    public void play(int animation) => a.Play(animation);
}