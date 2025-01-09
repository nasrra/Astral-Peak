using Deluz;
using UnityEngine;

public class MagicSlashProjectile : Projectile{
    [SerializeField] Transform front_point;
    [SerializeField] float speed;
    void Awake() => StartCoroutine(Util.timer(
        time: 5,
        time_out: destroy
    ));
    protected override void Start(){
        movement.movement_state(front_point.position-transform.position,speed);
        base.Start();
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.layer==LayersManager.PLAYER)
                damage_creature(other.GetComponent<Creature>());
        else
            destroy();
    }    
    public override void destroy(){
        base.destroy();
        Destroy(gameObject);
    }
}
