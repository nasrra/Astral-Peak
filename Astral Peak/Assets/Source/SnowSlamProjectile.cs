using Entropek;
using UnityEngine;

public class SnowSlamProjectile : Projectile{
    [SerializeField] Transform front_point;
    [SerializeField] float speed;
    AudioSource source;
    protected override void Start(){
        movement.movement_state(front_point.position-transform.position,speed);
        //StartCoroutine(Util.timer(
        //    time: 5,
        //    time_out: destroy
        //));
        base.Start();
        // source = AudioClipHandler.play(Sounds.SoundID.SNOW_ROLLING, gameObject, AudioSourceSettings.DIEGETIC_RANDOMISED);
    }

    void OnTriggerEnter2D(Collider2D other){
        int layer = other.gameObject.layer;
        if(layer==LayersManager.PLAYER)
            damage_creature(other.GetComponent<Creature>());
        else if(layer == LayersManager.PROJECTILE_DESTROYER)
            destroy();
        else
            destroy();
    }    
    public override void destroy(){
        // StartCoroutine(AudioClipHandler.fade_out(source, 2));
        Destroy(gameObject, 2.1f);
    }
}
