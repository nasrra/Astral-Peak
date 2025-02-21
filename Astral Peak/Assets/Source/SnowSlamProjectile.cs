using Entropek;
using UnityEngine;

public class SnowSlamProjectile : Projectile{
    [SerializeField] Transform front_point;
    [SerializeField] float speed;
    AudioSource source;
    protected override void Start(){
        movement.movement_state(front_point.position-transform.position,speed);
        audio_player.play_diegetic_loop("snow_rolling");
        base.Start();
    }

    protected override void OnTriggerEnter2D(Collider2D other){
        int layer = other.gameObject.layer;
        if(layer==LayersManager.PLAYER)
            damage_creature(other.GetComponent<Creature>());
        else if(layer == LayersManager.PROJECTILE_DESTROYER)
            destroy();
        else
            destroy();
    }    
    public override void destroy(){
        audio_player.stop_diegetic_loop("snow_rolling");
        Destroy(gameObject, 2.1f);
    }
}
