using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ConstellationController : ImageHandler{
    [SerializeField] Image image;

    public void fade_in() => StartCoroutine(fade_in_coroutine(2f));
    public void fade_out() => StartCoroutine(fade_out_coroutine(2f));
    public IEnumerator fade_in_coroutine(float time){
        yield return lerp_renderer_color("main", new Color(0,0,0,0), Color.white, time);
    }

    public IEnumerator fade_out_coroutine(float time){
        yield return lerp_renderer_color("main", Color.white, new Color(0,0,0,0), time);
    }
}
