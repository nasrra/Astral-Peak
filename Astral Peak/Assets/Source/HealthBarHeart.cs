using UnityEngine;

public class HealthBarHeart : ImageHandler{
    [SerializeField] Animator animator;
    State state = State.OFF;

    readonly int
        MAIN        = 0,
        OVERRIDE    = 1;

    public void fade_out() => animator.Play("fade_out", OVERRIDE);
    public void fade_in() => animator.Play("fade_in", OVERRIDE);
    public void disable(){
        if(state != State.OFF){
            state = State.OFF;
            animator.Play("disable", MAIN);
        }
        else animator.Play("disabled", MAIN);
    }
    public void off(){
        animator.Play("off", OVERRIDE, 0);
        animator.Update(0f);
    }
    public void enable(){
        if(state != State.ON){
            state = State.ON;
            animator.Play("enable", MAIN);
        }
        else animator.Play("enabled", MAIN);
    }
    public void thump(bool x) => animator.SetBool("thump", x);

    public void set_fill(float amount) => set_value("_amount", amount);

    enum State{
        ON,OFF,THUMP
    }
}