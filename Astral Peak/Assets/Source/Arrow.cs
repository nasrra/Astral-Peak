using UnityEngine;
using Sounds;
using Deluz;

public class Arrow : Projectile{
    [Header("Arrow")]
    [SerializeField] protected Transform front_point;
    [SerializeField] float move_time, rotation_speed, move_speed;
    void Start(){
        movement.movement_state(front_point.position-transform.position,move_speed);
        StartCoroutine(Util.timer(
            move_time, 
            time_out:()=>movement.rotate_to_direction_state(Vector2.down, rotation_speed))); 
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.layer == LayersManager.PLAYER)
            damage_creature_and_self_destruct(other.GetComponent<Creature>());
        else if(other.gameObject.layer == LayersManager.GROUND)
            StartCoroutine(
                Util.timer(
                    time: 1,
                    start_action:()=>grounded(),
                    time_out:()=>destroy()
                )
            );
    }

    void grounded(){
        movement.StopAllCoroutines();
        movement.zero_velocity();
        enable_colliders(false);
        particles.play_particle("grounded");
        AudioClipHandler.play(
            SoundID.SNOW_IMPACT_LIGHT,
            audio_player: this, 
            AudioSourceSettings.DIEGETIC);        
    }

    public override void destroy(){
        enable_sprites(false);
        ParticleSystem smoke = particles.get_particle("smoke");
        smoke.Play();
        AudioClipHandler.play(
            SoundID.STEAM,
            audio_player: this, 
            AudioSourceSettings.DIEGETIC);     
        Destroy(gameObject, smoke.GetComponent<ParticleSystem>().main.duration);    
    }
}
