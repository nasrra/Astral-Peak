using Entropek;
using UnityEngine;

public class MagicTrackingProjectile : MagicStationaryProjectile{
    [Header("MagicTrackingProjectile")]
    [SerializeField] Transform front_point;
    [SerializeField] Vector2 buffer_direction;
    [SerializeField] float rotate_speed, move_speed, buffer_time, buffer_speed;
    protected override void Start(){
        StartCoroutine(Util.timer(buffer_time,
            start_action: () => {
                enable_colliders(false);
                movement.rotate_to_target_loop_state(Player.instance.transform, rotate_speed);
            },
            time_out: () => {
                movement.move_to_target_state(front_point, move_speed);
                enable_colliders(true);
                transform.parent = null;
        }));
        base.Start();
    }
    protected override void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.layer == LayersManager.GROUND)
            destroy();
        else
            base.OnTriggerEnter2D(other);
    }
    void OnDisable(){}
    public override void destroy(){
        movement.StopAllCoroutines();
        movement.zero_velocity();
        base.destroy();
    }
}
