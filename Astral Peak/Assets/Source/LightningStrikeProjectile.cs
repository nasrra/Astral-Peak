using Entropek;
using UnityEngine;

public class LightningStrikeProjectile : Projectile{
    [Header("Lightning Strike")]
    [SerializeField] LineParticleEmitter lightning;
    [SerializeField] float lifetime, move_speed;
    [SerializeField] SpriteHandler sprite_handler;

    public override void destroy(){
        Destroy(gameObject);
    }

    void Awake(){
        enable_colliders(false);
        off();
        turn_on();
    }

    protected override void Start(){
        StartCoroutine(Util.timer(
            time: 1.75f,
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
    
    protected override void OnTriggerEnter2D(Collider2D other){
        int layer = other.gameObject.layer; 
        if(layer == LayersManager.PLAYER)
            damage_creature(other.GetComponent<Creature>());
        else if(layer == LayersManager.PROJECTILE_DESTROYER)
            destroy();
    }
    public void off(){
        sprite_handler.set_value("_Dim",10);
        sprite_handler.enable_sprite(false);
    }

    public void turn_on(){
        sprite_handler.enable_sprite(true);
        StartCoroutine(sprite_handler.lerp_value(
            value: "_Dim",
            time: 1.5f,
            start:10,
            end:1
        ));        
    }
}
