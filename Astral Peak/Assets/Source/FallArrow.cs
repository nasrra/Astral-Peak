using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class FallArrow : Arrow{
    [SerializeField] float
        rise_speed,
        rise_time, 
        rotate_speed,
        rotate_time,
        adjust_speed,
        fall_speed;

    Coroutine coroutine;

    void Awake() {} // we dont want a life time or movement of the normal projectile class
    void Start() => coroutine = StartCoroutine(fall_arrow_behaviour());

    IEnumerator fall_arrow_behaviour(){
        float rng = Random.Range(0,31);
        rng /= 100;
        rng /= 2;
        rise_speed      += rng;
        rise_time       += rng; 
        rotate_speed    += rng;
        rotate_time     += rng;
        adjust_speed    += rng;
        fall_speed      += rng;
        state_switch(ref move_state, move(rise_speed, rise_time));
        yield return new WaitForSeconds(rise_time);
        state_switch(ref rotate_state, rotate_to_target(Player.instance.transform, rotate_speed, rotate_time));
        state_switch(ref move_state, move(adjust_speed));
        yield return new WaitForSeconds(rotate_time);
        state_switch(ref move_state, move(fall_speed));
        yield break;
    }
}
