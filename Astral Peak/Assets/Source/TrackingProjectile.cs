using System.Collections;
using UnityEngine;

public class TrackingProjectile : Projectile{
    [SerializeField] float
        buffer_time,
        snap_rotate_speed,
        lerp_rotate_speed;

    void Awake(){}
    void Start(){
        state_switch(rotate_state, rotate_to_target(snap_rotate_speed));
        StartCoroutine(buffer_timer());
    }

    IEnumerator buffer_timer(){
        yield return new WaitForSeconds(buffer_time);
        state_switch(move_state, move(move_speed));
        state_switch(rotate_state, rotate_to_target(lerp_rotate_speed));
        enable_trail(true);
        yield break;
    }
}
