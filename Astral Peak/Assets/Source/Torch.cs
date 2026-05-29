using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Torch : MonoBehaviour{
    [SerializeField] Light2D light2D;
    [SerializeField] List<SpriteRenderer> fire;
    [SerializeField] ParticleSystem smoke, embers;
    Coroutine light_state, fire_state;
    [SerializeField] Vector3 enlarged_scale, original_scale;
    [SerializeField] AudioPlayer audio_player;
    bool turned_on = false;

    void Awake(){
        original_scale = fire[0].transform.localScale;
    }

    void state_switch(ref Coroutine state, IEnumerator _state){
        if(state != null)
            StopCoroutine(state);
        state = StartCoroutine(_state);
    }
    public void enlargen(){
        embers.Emit(60);
        StartCoroutine(enalargen_flame(2));
        StartCoroutine(enalargen_light(2));
    }
    public void revert(){
        StartCoroutine(reset_flame(2));
        StartCoroutine(reset_light(2));
    }

    public void on(){
        turned_on = true;
        fire[0].material.SetFloat("_Dim", 1);
        light2D.intensity = 2;
        audio_player.play_diegetic_loop("fire_crackle_soft");
        foreach(SpriteRenderer s in fire)
            s.enabled = true;   
        light2D.enabled = true;  
    }   

    public void turn_on(){
        if(turned_on == true)
            return;
        turned_on = true;
        state_switch(ref light_state, turn_on_light());
        state_switch(ref fire_state, turn_on_fire());
    }
    public void turn_off(){
        if(turned_on == false)
            return;
        turned_on = false;
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
            s.enabled = false;
        }
        smoke.Play(); 
        audio_player.play_diegetic_one_shot("fire_extinguish");
        audio_player.stop_all_loops();
        embers.Emit(30);
        embers.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        yield break;
    }

    IEnumerator turn_on_fire() {
        float x = 1;
        audio_player.play_diegetic_loop("fire_crackle_soft");
        embers.Emit(30);
        embers.Play();
        foreach(SpriteRenderer s in fire){
            s.material.SetFloat("_Dim", 10);
            s.enabled = true;
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

    IEnumerator enalargen_flame(float speed){
        float timer = 0;
        while(fire[0].transform.localScale.magnitude < enlarged_scale.magnitude){
            timer += Time.deltaTime;
            fire[0].transform.localScale = Vector3.Lerp(original_scale, enlarged_scale, timer * speed);
            yield return null; 
        }
        fire[0].transform.localScale = enlarged_scale;
        yield break;
    }

    IEnumerator reset_flame(float speed){
        float timer = 0;
        while(fire[0].transform.localScale.magnitude > original_scale.magnitude){
            timer += Time.deltaTime;
            fire[0].transform.localScale = Vector3.Lerp(enlarged_scale, original_scale, timer * speed);
            yield return null; 
        }
        fire[0].transform.localScale = original_scale;
        yield break;        
    }

    IEnumerator enalargen_light(float speed){
        float timer = 0;
        while(light2D.transform.localScale.magnitude < enlarged_scale.magnitude){
            timer += Time.deltaTime;
            light2D.transform.localScale = Vector3.Lerp(original_scale, enlarged_scale, timer * speed);
            yield return null; 
        }
        light2D.transform.localScale = enlarged_scale;
        yield break;
    }

    IEnumerator reset_light(float speed){
        float timer = 0;
        while(light2D.transform.localScale.magnitude > original_scale.magnitude){
            timer += Time.deltaTime;
            light2D.transform.localScale = Vector3.Lerp(enlarged_scale, original_scale, timer * speed);
            yield return null; 
        }
        light2D.transform.localScale = original_scale;
        yield break;        
    }
    IEnumerator turn_on_light(){
        light2D.enabled = true;
        light2D.intensity = 0;
        while(light2D.intensity < 2.5f){
            light2D.intensity += Time.deltaTime;
            yield return null;
        }
        light2D.intensity = 2;
        yield break;
    }
    IEnumerator turn_off_light(){
        while(light2D.intensity > 0){
            light2D.intensity -= Time.deltaTime;
            yield return null;
        }
        light2D.intensity = 0;
        light2D.enabled = false;
        yield break;
    }
}
