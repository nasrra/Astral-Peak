
using UnityEngine;

public class CavalryAnimator : MonoBehaviour{
    [SerializeField] Animator a;
    public readonly int 
        IDLE    = Animator.StringToHash("idle"),
        RUN     = Animator.StringToHash("run"),
        NONE    = Animator.StringToHash("none"),
        FRONT_SWING = Animator.StringToHash("front_swing"),
        BACK_SWING  = Animator.StringToHash("back_swing"),
        BITE_1 = Animator.StringToHash("bite_1"),
        BITE_2 = Animator.StringToHash("bite_2");

    public void idle() => a.Play(IDLE);
    public void run() => a.Play(RUN);
    public void front_swing() => a.Play(FRONT_SWING);
    public void back_swing() => a.Play(BACK_SWING);
    public void bite() => a.Play(BITE_1);
}
