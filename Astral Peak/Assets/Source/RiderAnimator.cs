using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiderAnimator : AnimatorOverride{
    public void idle() => animator.Play("idle");
    public void yell() => animator.Play("yell");
    public void run() => animator.Play("run");
    public void whistle() => animator.Play("whistle");
}
