using UnityEngine;

public class MagicStationaryProjectile : Projectile{
    [Header("Magic Stationary Projectile")]
    [SerializeField] ParticleSystem ambience;
    [SerializeField] protected TrailRenderer trail;
    [SerializeField] bool destroy_on_hit = true;
    AudioSource source;

    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.layer==LayersManager.PLAYER)
            if(destroy_on_hit == true)
                damage_creature_and_self_destruct(other.GetComponent<Creature>());
            else
                damage_creature(other.GetComponent<Creature>());
        else if(destroy_on_hit == true)
            destroy();
    }

    protected void play_sound() =>
        source = AudioClipHandler.play(
            Sounds.SoundID.ELECTRICITY_3,
            this,
            AudioSourceSettings.DIEGETIC_RANDOMISED_LOOP);
    protected void stop_sound() => 
        AudioClipHandler.fade_out(
        this,
        source,
        ambience.main.duration/8);  

    public override void destroy(){
        ambience.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        stop_sound();
        enable_colliders(false);
        enable_sprites(false);
        Destroy(gameObject, ambience.main.duration);    
    }
}
