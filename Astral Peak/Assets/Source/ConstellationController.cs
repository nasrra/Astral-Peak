using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ConstellationController : MonoBehaviour{
    [SerializeField] Image image;
    [SerializeField] float shader_amount;

    public void fade_in() => StartCoroutine(fade_in_coroutine(.01f));
    public void fade_out() => StartCoroutine(fade_out_coroutine(.02f));
    public IEnumerator fade_in_coroutine(float speed){
        image.enabled = true;
        float x = image.material.GetFloat("_amount");
        while(x < shader_amount){
            image.material.SetFloat("_amount", x += Time.deltaTime * speed);
            x = image.material.GetFloat("_amount");
            yield return null;
        }
        image.material.SetFloat("_amount", shader_amount);
        yield break;
    }

    public IEnumerator fade_out_coroutine(float speed){
        float x = image.material.GetFloat("_amount");
        while(x > 0){
            image.material.SetFloat("_amount", x -= Time.deltaTime * speed);
            x = image.material.GetFloat("_amount");
            yield return null;
        }
        x=0;
        image.material.SetFloat("_amount", x);
        image.enabled = false;
        yield break;
    }

}
