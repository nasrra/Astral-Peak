using System;
using System.Collections.Generic;
using UnityEngine;

public class CavalryAnimator : AnimatorOverride{
    public static readonly int
        IDLE                    = Animator.StringToHash("idle"),
        RUN                     = Animator.StringToHash("run"),
        FRONT_STRIKE            = Animator.StringToHash("front_strike"), 
        BACK_STRIKE_BACKWARD    = Animator.StringToHash("back_strike_backward"),
        BACK_STRIKE_FORWARD     = Animator.StringToHash("back_strike_forward"),
        FETCH_1                 = Animator.StringToHash("fetch_1"), 
        BITE_1                  = Animator.StringToHash("bite_1"), 
        BITE_2                  = Animator.StringToHash("bite_2"),
        BITE_3                  = Animator.StringToHash("bite_3"),
        WHISTLE                 = Animator.StringToHash("whistle"),
        NO_SWORD_IDLE           = Animator.StringToHash("no_sword_idle"),
        NO_SWORD_RUN            = Animator.StringToHash("no_sword_run"),
        PICKUP_SWORD            = Animator.StringToHash("pickup_sword"),
        HOWL                    = Animator.StringToHash("howl"),
        GROUND_SLAM             = Animator.StringToHash("ground_slam");

    // animator key events:
    public override void idle()         => animator.Play(IDLE);
    public override void run()          => animator.Play(RUN);
    public void front_strike()          => animator.Play(FRONT_STRIKE);
    public void back_strike_backward()  => animator.Play(BACK_STRIKE_BACKWARD);     
    public void back_strike_forward()   => animator.Play(BACK_STRIKE_FORWARD);  
    public void bite_1()                => animator.Play(BITE_1);
    public void bite_2()                => animator.Play(BITE_2);
    public void bite_3()                => animator.Play(BITE_3); 
    public void fetch_1()               => animator.Play(FETCH_1);
    public void whistle()               => animator.Play(WHISTLE);
    public void howl()                  => animator.Play(HOWL);
    public void no_sword_idle()         => animator.Play(NO_SWORD_IDLE);
    public void no_sword_run()          => animator.Play(NO_SWORD_RUN);
    public void pickup_sword()          => animator.Play(PICKUP_SWORD);
    public void ground_slam()           => animator.Play(GROUND_SLAM);
}

