using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarHeart : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] Animator animator;
    State state = State.OFF;
    public void fade_out(){
        animator.enabled = false;
        StartCoroutine(ValueHelper.lerp_image_colour(image, new Color(0,0,0,0), 1));
    }
    public void fade_in() => StartCoroutine(fade_in_coroutine());
    IEnumerator fade_in_coroutine(){
        StartCoroutine(ValueHelper.lerp_image_colour(image, Color.white, 1));
        yield return new WaitForSeconds(1);
        animator.enabled = true;
    }
    public void turn_off(){
        if(state != State.OFF){
            state = State.OFF;
            animator.Play("turn_off");
        }
    }
    public void turn_on(){
        if(state != State.ON){
            state = State.ON;
            animator.Play("turn_on");
        }    
    }
    public void thump(bool x) => animator.SetBool("thump", x);

    enum State{
        ON,OFF,THUMP
    }
}
