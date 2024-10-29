using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CavalryAnimator : MonoBehaviour{
    [SerializeField] Animator animator;
    public delegate void AnimationDelegate();

    public Dictionary<int, AnimationDelegate> animations = new Dictionary<int, AnimationDelegate>();

    public static readonly int
        IDLE = Animator.StringToHash("idle"),
        RUN = Animator.StringToHash("run"),
        FRONT_STRIKE = Animator.StringToHash("front_strike"), 
        BACK_STRIKE = Animator.StringToHash("back_strike"), 
        BITE_1 = Animator.StringToHash("bite_1"), 
        BITE_2 = Animator.StringToHash("bite_2"),
        BITE_3 = Animator.StringToHash("bite_3");

    void Awake() => 
        animations = new Dictionary<int, AnimationDelegate>(){
            {IDLE, idle},
            {RUN, run},
            {FRONT_STRIKE, front_strike},
            {BACK_STRIKE, back_strike},
            {BITE_1, bite_1},
            {BITE_2, bite_2},
            {BITE_3, bite_3},
        };

    public void play(int animation_id) => animations[animation_id]();

    void idle() => animator.Play(IDLE);
    void run() => animator.Play(RUN);
    void back_strike() => animator.Play(BACK_STRIKE);
    void front_strike() => animator.Play(FRONT_STRIKE);
    void bite_1() => animator.Play(BITE_1);
    void bite_2() => animator.Play(BITE_2);
    void bite_3() => animator.Play(BITE_3);
}

