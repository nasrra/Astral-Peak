using Entropek;
using UnityEngine;

public class LightningStrikeProjectile : Projectile{
    [Header("Lightning Strike")]
    [SerializeField] LineParticleEmitter lightning;
    [SerializeField] float lifetime, move_speed;

    public override void destroy(){
        Destroy(gameObject);
    }

    void Awake(){
        enable_colliders(false);
    }

    protected override void Start(){
        //StartCoroutine(Util.timer(
        //    time: lifetime,
        //    time_out: destroy
        //));
        StartCoroutine(Util.timer(
            time: 1.5f,
            time_out: loop
        ));
        snap_to_floor();
        base.Start();
    }
    void loop(){
        enable_colliders(true);
        lightning.start_emitting();
        movement.movement_state(Player.instance.transform.position.x - transform.position.x <= 0? Vector2.left : Vector2.right, move_speed); 
    }
    
    void OnTriggerEnter2D(Collider2D other){
        int layer = other.gameObject.layer; 
        if(layer == LayersManager.PLAYER)
            damage_creature(other.GetComponent<Creature>());
        else if(layer == LayersManager.PROJECTILE_DESTROYER)
            destroy();
    }
}
