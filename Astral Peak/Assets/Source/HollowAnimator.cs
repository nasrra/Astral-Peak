using UnityEngine;

public class HollowAnimator : AnimatorOverride{
    [SerializeField] private Animator a;
    public static readonly int
        IDLE        = Animator.StringToHash("idle"),
        ATTACK      = Animator.StringToHash("attack"),
        GUARD       = Animator.StringToHash("guard"),
        STUNNED     = Animator.StringToHash("stunned");


    public void idle()      => a.Play(IDLE);
    public void attack()    => a.Play(ATTACK);
    public void guard()     => a.Play(GUARD);
    public void stunned()   => a.Play(STUNNED);
    public void play(int animation) => a.Play(animation);
}