using System.Collections;
using UnityEngine;

public class FallArrow : Arrow{
    [Header("Fall Arrow")]
    [SerializeField] float
        rise_speed,
        rise_time, 
        rotate_speed,
        rotate_time,
        fall_speed;


    void Start() => StartCoroutine(fall_arrow_behaviour());

    IEnumerator fall_arrow_behaviour(){
        float rng = Random.Range(0,16);
        rng /= 100;
        rise_speed      += rng;
        rise_time       += rng; 
        rotate_speed    += rng;
        rotate_time     += rng;
        fall_speed      += rng;
        movement.movement_state(front_point.position - transform.position, rise_speed, rise_time);
        yield return new WaitForSeconds(rise_time);
        movement.StopAllCoroutines();
        movement.zero_velocity();
        movement.rotate_to_target_state(Player.instance.transform, rotate_speed);
        yield return new WaitForSeconds(rotate_time);
        movement.movement_state(front_point.position - transform.position, fall_speed);
        yield break;
    }
}
