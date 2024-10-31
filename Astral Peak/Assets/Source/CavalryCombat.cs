using System;
using System.Collections.Generic;
using UnityEngine;

public class CavalryAnimator : MonoBehaviour{
    [SerializeField] Animator animator;
    
    public static readonly int
        IDLE            = Animator.StringToHash("idle"),
        RUN             = Animator.StringToHash("run"),
        FRONT_STRIKE    = Animator.StringToHash("front_strike"), 
        BACK_STRIKE     = Animator.StringToHash("back_strike"),
        FETCH_1         = Animator.StringToHash("fetch_1"), 
        BITE_1          = Animator.StringToHash("bite_1"), 
        BITE_2          = Animator.StringToHash("bite_2"),
        BITE_3          = Animator.StringToHash("bite_3"),
        WHISTLE         = Animator.StringToHash("whistle"),
        NO_SWORD_IDLE   = Animator.StringToHash("no_sword_idle"),
        NO_SWORD_RUN    = Animator.StringToHash("no_sword_run");

    [SerializeField] List<BossAttack> 
        front_moveset = new List<BossAttack>(), 
        back_moveset = new List<BossAttack>();

    void Start(){
        set_moveset_transforms();
        set_moveset_animations();
    }

    void set_moveset_animations(){
        front_moveset[0].set_animation(()=>animator.Play(FRONT_STRIKE));
        front_moveset[1].set_animation(()=>animator.Play(BITE_1));
        front_moveset[2].set_animation(()=>animator.Play(FETCH_1));

        back_moveset[0].set_animation(()=>animator.Play(BACK_STRIKE));
    }

    void set_moveset_transforms(){
        foreach(BossAttack a in front_moveset)
            a.set_transform(transform);
        foreach(BossAttack a in back_moveset)
            a.set_transform(transform);
    }
    
    public List<BossAttack> get_front_moveset() => front_moveset; 
    public List<BossAttack> get_back_moveset() => back_moveset;


    // animator key events:
    public void run()           => animator.Play(RUN);
    public void idle()          => animator.Play(IDLE);
    public void bite_2()        => animator.Play(BITE_2);
    public void bite_3()        => animator.Play(BITE_3); 
    public void whistle()       => animator.Play(WHISTLE);
    public void no_sword_idle() => animator.Play(NO_SWORD_IDLE);
    public void no_sword_run() => animator.Play(NO_SWORD_RUN);
}

