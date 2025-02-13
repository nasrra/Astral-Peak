using System.Collections;
using UnityEngine;

public class Beatrice : MonoBehaviour{
    [SerializeField] Animator animator;
    [SerializeField] SpriteHandler sprite;

    void Awake(){
        idle();
    } 

    public void idle(){
        animator.Play("idle",0);
        animator.Play("idle",1);
    }

    public void awaken()=>StartCoroutine(awaken_coroutine());

    IEnumerator awaken_coroutine(){
        StartCoroutine(sprite.lerp_value("_charge_amount",0, .15f, 1f));
        yield return new WaitForSeconds(2);
        StartCoroutine(sprite.lerp_value("_charge_amount",.15f, 0, 1f));
        yield return new WaitForSeconds(2);
        animator.Play("awaken",1);
        yield break;
    }
}
