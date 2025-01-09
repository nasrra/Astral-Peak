using System.Collections;
using Deluz;
using UnityEngine;

public class MagicTrackingProjectile : MagicStationaryProjectile{
    [Header("MagicTrackingProjectile")]
    [SerializeField] Transform front_point;
    [SerializeField] float rotate_speed, move_speed, buffer_time;
    protected override void Start(){
        StartCoroutine(Util.timer(buffer_time,
            start_action: () => {
                enable_colliders(false);
                trail.enabled = false;   
                movement.rotate_to_target_loop_state(Player.instance.transform, rotate_speed);
            },
            time_out: () => {
                movement.move_to_target_state(front_point, move_speed);
                trail.enabled=true;
                enable_colliders(true);
                transform.parent = null;
        }));
        base.Start();
    }
    public override void destroy(){
        movement.StopAllCoroutines();
        base.destroy();
    }
}
