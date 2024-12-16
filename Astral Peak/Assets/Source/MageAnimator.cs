using UnityEngine;

public class MageAnimator : AnimatorOverride{
    readonly int
        MAIN        = 0,
        OVERRIDE    = 1;
    
    public void idle() => animator.Play("idle", MAIN);
    public void invisible(bool x) => animator.Play(x==true?"enter_invisible":"exit_invisible", OVERRIDE);
}
