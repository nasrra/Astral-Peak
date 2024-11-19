using System.Collections.Generic;
using UnityEditor.SearchService;
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
        GROUND_SLAM             = Animator.StringToHash("ground_slam"),
        JUMP_AWAY               = Animator.StringToHash("jump_away"),
        PHASE_TRANSITION        = Animator.StringToHash("phase_transition"),
        DEATH                   = Animator.StringToHash("death");

    // animator key events:
    public void idle()                  => animator.Play(IDLE);
    public void run()                   => animator.Play(RUN);
    public void front_strike()          => animator.Play(FRONT_STRIKE);
    public void back_strike_backward()  => animator.Play(BACK_STRIKE_BACKWARD);     
    public void back_strike_forward()   => animator.Play(BACK_STRIKE_FORWARD);  
    public void bite_1()                => animator.Play(BITE_1);
    public void bite_2()                => rng(100,BITE_2);
    public void bite_3()                => rng(100,BITE_3); 
    public void fetch_1()               => animator.Play(FETCH_1);
    public void whistle()               => animator.Play(WHISTLE);
    public void howl()                  => animator.Play(HOWL);
    public void no_sword_idle()         => animator.Play(NO_SWORD_IDLE);
    public void no_sword_run()          => animator.Play(NO_SWORD_RUN);
    public void pickup_sword()          => animator.Play(PICKUP_SWORD);
    public void ground_slam()           => animator.Play(GROUND_SLAM);
    public void death()                 => animator.Play(DEATH);
    static public string phase_transition(int phase) => "phase_transition_"+phase;

    void rng(float chance, int id){
        if(Random.Range(0, 101) < chance)
            animator.Play(id);
    }
}

