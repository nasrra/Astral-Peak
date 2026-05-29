using Entropek;
using UnityEngine;

public class HealthBarHeart : SpriteHandler{
    [SerializeField] Animator animator;
    State state = State.OFF;
    Coroutine flash_state;

    readonly int
        MAIN        = 0,
        OVERRIDE    = 1;

    public void fade_out(){
        animator.Play("fade_out", OVERRIDE,0);
    }
    public void fade_in(){
        animator.Play ("fade_in", OVERRIDE,0);
    }
    public void disable(){
        if(state != State.OFF){
            state = State.OFF;
            animator.Play("disable", MAIN,0);
        }
        else animator.Play("disabled", MAIN,0);
    }
    public void off(){
        animator.Play("off", OVERRIDE, 0);
        animator.Update(0f);
    }
    public void enable(){
        if(state != State.ON){
            state = State.ON;
            animator.Play("enable", MAIN,0);
        }
        else animator.Play("enabled", MAIN,0);
    }
    public void thump(bool x) => animator.SetBool("thump", x);
    public void disabled_thump(){
        animator.Play("disabled_thump");
    }

    public void set_fill(float amount) => set_value("_amount", amount);

    public void health_gained_flash(){
        state_switch(ref flash_state, pulse_value("_intensity", 2f, 1f, 0.5f, 1));
    }

    enum State{
        ON,OFF,THUMP
    }
}