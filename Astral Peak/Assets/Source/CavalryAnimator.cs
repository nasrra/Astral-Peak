using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UIElements;

public class CavalryAnimator : MonoBehaviour{
    [SerializeField] Animator animator;
    public delegate void AnimationDelegate();

    public Dictionary<int, AnimationDelegate> animations = new Dictionary<int, AnimationDelegate>();

    public static readonly int
        IDLE = Animator.StringToHash("idle"),
        RUN = Animator.StringToHash("run"),
        FRONT_STRIKE = Animator.StringToHash("front_strike"), 
        BACK_STRIKE = Animator.StringToHash("back_strike"),
        FETCH_1 = Animator.StringToHash("fetch_1"), 
        BITE_1 = Animator.StringToHash("bite_1"), 
        BITE_2 = Animator.StringToHash("bite_2"),
        BITE_3 = Animator.StringToHash("bite_3"),
        WHISTLE = Animator.StringToHash("whistle"),
        NO_SWORD_IDLE = Animator.StringToHash("no_sword_idle"),
        NO_SWORD_RUN = Animator.StringToHash("no_sword_run");

    void Awake() => 
        animations = new Dictionary<int, AnimationDelegate>(){
            {IDLE,          ()=>animator.Play(IDLE)},
            {RUN,           ()=>animator.Play(RUN)},
            {FRONT_STRIKE,  ()=>animator.Play(FRONT_STRIKE)},
            {BACK_STRIKE,   ()=>animator.Play(BACK_STRIKE)},
            {FETCH_1,       ()=>animator.Play(FETCH_1)},
            {BITE_1,        ()=>animator.Play(BITE_1)},
            {BITE_2,        ()=>animator.Play(BITE_2)},
            {BITE_3,        ()=>animator.Play(BITE_3)},
            {WHISTLE,       ()=>animator.Play(WHISTLE)},
            {NO_SWORD_IDLE, ()=>animator.Play(NO_SWORD_IDLE)},
            {NO_SWORD_RUN,  ()=>animator.Play(NO_SWORD_RUN)}
        };
    public void play(int animation_id) => animations[animation_id]();
    
    // animator key events:
    public void bite_2() => play(BITE_2);
    public void bite_3() => play(BITE_3); 
    public void whistle() => play(WHISTLE);
}

