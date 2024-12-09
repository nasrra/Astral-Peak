using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarHeart : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] Animator animator;
    State state = State.OFF;

    readonly int
            MAIN        = 0,
            OVERRIDE    = 1;

    public void fade_out() => animator.Play("fade_out", OVERRIDE);
    public void fade_in() => animator.Play("fade_in", OVERRIDE);
    public void turn_off(){
        if(state != State.OFF){
            state = State.OFF;
            animator.Play("turn_off", MAIN);
        }
        else animator.Play("off", MAIN);
    }
    public void turn_on(){
        if(state != State.ON){
            state = State.ON;
            animator.Play("turn_on", MAIN);
        }
        else animator.Play("on", MAIN);
    }
    public void thump(bool x) => animator.SetBool("thump", x);

    enum State{
        ON,OFF,THUMP
    }
}
