using System.Collections;
using UnityEngine;

public class TrackingProjectile : Projectile{
    [SerializeField] protected float
        buffer_time,
        snap_rotate_speed,
        lerp_rotate_speed;
    [SerializeField] bool enable_col_before_buffer = true;

    void Awake(){}
    void Start(){
        state_switch(ref rotate_state, rotate_to_target(Player.instance.transform, snap_rotate_speed));
        StartCoroutine(buffer_timer());
    }

    protected IEnumerator buffer_timer(){
        col.enabled = enable_col_before_buffer;
        yield return new WaitForSeconds(buffer_time);
        state_switch(ref move_state, move(move_speed));
        state_switch(ref rotate_state, rotate_to_target(Player.instance.transform, lerp_rotate_speed));
        enable_trail(true);
        col.enabled = true;
        yield break;
    }
}
