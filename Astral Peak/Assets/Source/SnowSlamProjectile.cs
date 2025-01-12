using Entropek;
using UnityEngine;

public class SnowSlamProjectile : Projectile{
    [SerializeField] Transform front_point;
    [SerializeField] float speed;
    protected override void Start(){
        movement.movement_state(front_point.position-transform.position,speed);
        StartCoroutine(Util.timer(
            time: 5,
            time_out: destroy
        ));
        base.Start();
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.layer==LayersManager.PLAYER)
                damage_creature(other.GetComponent<Creature>());
        else
            destroy();
    }    
    public override void destroy(){
        Destroy(gameObject);
    }
}
