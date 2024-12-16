using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

public class CavalryAnimator : AnimatorOverride{
    //IDLE                    = Animator.StringToHash("idle"),

    // animator key events:
    public void idle()                  => animator.Play("idle");
    public void run()                   => animator.Play("run");
    public void front_strike()          => animator.Play("front_strike");
    public void back_strike_backward()  => animator.Play("back_strike_backward");     
    public void back_strike_forward()   => animator.Play("back_strike_backward"); 
    public void bite_1()                => animator.Play("bite_1");
    public void bite_2()                => animator.Play("bite_2");
    public void bite_3()                => animator.Play("bite_3"); 
    public void fetch_1()               => animator.Play("fetch_1");
    public void whistle()               => animator.Play("whistle");
    public void howl()                  => animator.Play("howl");
    public void ground_slam()           => animator.Play("ground_slam");
    public void death()                 => animator.Play("death");
    static public string phase_transition(int phase) => "phase_transition_"+phase;

    void rng(float chance, int id){
        if(Random.Range(0, 101) <= chance)
            animator.Play(id);
    }
}

