using UnityEngine;

public class PhaseTransitionArrow : Projectile{
    [Header("Arrow")]
    [SerializeField] ParticleSystem smoke, snow;
    [SerializeField] protected Transform front_point;
    [SerializeField] float move_speed;
    protected override void Start(){
        movement.move_to_target_state(front_point, move_speed);
        base.Start();
    }

    public override void destroy(){
        Destroy(gameObject);
    }
}
