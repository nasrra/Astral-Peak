using Entropek;
using UnityEngine;

public class MagicStationaryProjectile : Projectile{
    [Header("Magic Stationary Projectile")]
    [SerializeField] ParticleSystem ambience;
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] protected TrailRenderer trail;
    [SerializeField] bool destroy_on_hit = true;

    protected virtual void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.layer==LayersManager.PLAYER)
            if(destroy_on_hit == true)
                damage_creature_and_self_destruct(other.GetComponent<Creature>());
            else
                damage_creature(other.GetComponent<Creature>());
        else if(destroy_on_hit == true)
            destroy();
    }

    protected virtual void play_sound(){
        audio_player.play_diegetic_one_shot("electricity_burst_soft");
        audio_player.play_diegetic_loop("electricity_crackle_soft");
    }
    protected void stop_sound(){
        Log.MethodCall(); 
        audio_player.stop_loop("electricity_crackle_soft");  
    }

    void OnEnable(){
        play_sound();
    }

    void OnDisable(){
        stop_sound();    
    }

    public override void destroy(){
        ambience.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        stop_sound();
        enable_colliders(false);
        enable_sprites(false);
        Destroy(gameObject, 2);    
    }
}
