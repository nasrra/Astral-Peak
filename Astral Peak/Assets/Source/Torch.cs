using System.Collections;
using System.Collections.Generic;
using Mono.Cecil;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Torch : MonoBehaviour{
    [SerializeField] Light2D light2D;
    [SerializeField] List<SpriteRenderer> fire;
    [SerializeField] ParticleSystem smoke;
    Coroutine light_state, fire_state;

    void state_switch(ref Coroutine state, IEnumerator _state){
        if(state != null)
            StopCoroutine(state);
        state = StartCoroutine(_state);
    }
    public void turn_on(){
        state_switch(ref light_state, turn_on_light());
        state_switch(ref fire_state, turn_on_fire());
    }
    public void turn_off(){
        fire[0].material.SetFloat("_Dim", 1);
        state_switch(ref light_state, turn_off_light());
        state_switch(ref fire_state, turn_off_fire());
    }

    IEnumerator turn_off_fire() {
        while (Mathf.Abs(fire[0].material.GetFloat("_Dim") - 10) > 1f) {
            // Interpolate smoothly towards the target value based on transition speed
            float newDim = Mathf.Lerp(fire[0].material.GetFloat("_Dim"), 10, Time.deltaTime * 2);
            foreach(SpriteRenderer s in fire)
                s.material.SetFloat("_Dim", newDim);
            yield return null;
        }
        foreach(SpriteRenderer s in fire){
            s.material.SetFloat("_Dim", 10);
            s.gameObject.SetActive(false);
            smoke.Play();
        }
        yield break;
    }

    IEnumerator turn_on_fire() {
        float x = 1;
        foreach(SpriteRenderer s in fire){
            s.material.SetFloat("_Dim", 10);
            s.gameObject.SetActive(true);
        }
        while (Mathf.Abs(fire[0].material.GetFloat("_Dim") - x) > 0.5f) {
            float newDim = Mathf.Lerp(fire[0].material.GetFloat("_Dim"), 1, Time.deltaTime * 2);
            foreach(SpriteRenderer s in fire)
                s.material.SetFloat("_Dim", newDim);
            yield return null;
        }
        foreach(SpriteRenderer s in fire){
            s.material.SetFloat("_Dim", 1);
        }
        yield break;
    }
    IEnumerator test(){
        while(true){
            turn_on();
            yield return new WaitForSeconds(3);
            turn_off();
            yield return new WaitForSeconds(3);            
        }
    }
    IEnumerator turn_on_light(){
        light2D.gameObject.SetActive(true);
        while(light2D.intensity < 2.5f){
            light2D.intensity += Time.deltaTime *2;
            yield return null;
        }
        light2D.intensity = 2;
        yield break;
    }
    IEnumerator turn_off_light(){
        while(light2D.intensity > 0){
            light2D.intensity -= Time.deltaTime *2;
            yield return null;
        }
        light2D.intensity = 0;
        light2D.gameObject.SetActive(false);
        yield break;
    }
}
