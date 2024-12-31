using Deluz;
using UnityEngine;

public class MagicTrackingProjectile : Projectile{
    [Header("MagicTrackingProjectile")]
    [SerializeReference] Transform front_point;
    [SerializeField] TrailRenderer trail;
    [SerializeField] float rotate_speed, move_speed, buffer_time;
    AudioSource source;
    void Start(){
        StartCoroutine(Util.timer(buffer_time,
            start_action: () => {
                enable_colliders(false);   
                movement.rotate_to_target_loop_state(Player.instance.transform, rotate_speed);
            },
            time_out: () => {
                movement.move_to_target_state(front_point, move_speed);
                trail.enabled=true;
                enable_colliders(true);
                transform.parent = null;
        }));
        source = AudioClipHandler.play(
            Sounds.SoundID.ELECTRICITY_3,
            this,
            AudioSourceSettings.DIEGETIC_RANDOMISED_LOOP
        );
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.layer==LayersManager.PLAYER)
            damage_creature_and_self_destruct(other.GetComponent<Creature>());
        else
            destroy();
    }

    public override void destroy(){
        ParticleSystem ambience = particles.get_particle("ambience");
        ambience.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        AudioClipHandler.fade_out(
            this,
            source,
            ambience.main.duration/8
        );  
        enable_colliders(false);
        enable_sprites(false);
        movement.StopAllCoroutines();
        movement.zero_velocity();
        Destroy(gameObject, ambience.main.duration);
    }
}
