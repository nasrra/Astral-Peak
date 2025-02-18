using UnityEngine;
using Entropek;

public class SludgeTrackingProjectile : Projectile{
    [Header("SludgeTrackingProjectile")]
    [SerializeField] TrailRenderer trail;
    [SerializeField] ParticleSystem ambience;
    [SerializeField] ParticleSystem destroy_splash;
    [SerializeField] Transform front_point, top_point;
    [SerializeField] Vector2 buffer_direction;
    [SerializeField] float rotate_speed, move_speed, buffer_time, buffer_speed;
    protected override void Start(){
        play_sound();
        StartCoroutine(Util.timer(buffer_time,
            start_action: () => {
                enable_colliders(false);
                trail.enabled=true;
                transform.rotation = Quaternion.Euler(new Vector3(0,0,90));
                movement.move_to_target_state(front_point, move_speed);
            },
            time_out: () => {
                movement.rotate_to_target_loop_state(Player.instance.transform, rotate_speed);
                movement.move_to_target_state(front_point, move_speed);
                enable_colliders(true);
                transform.parent = null;
        }));
        base.Start();
    }
    protected override void play_sound(){
        audio_player.play_diegetic_loop("water_gurgle");
    } 

    protected override void stop_sound(){
        audio_player.play_diegetic_one_shot("water_splash_light");
        audio_player.stop_loop("water_gurgle");
    }
    
    protected override void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.layer == LayersManager.GROUND)
            destroy();
        else
            base.OnTriggerEnter2D(other);
    }
    public override void destroy(){
        base.destroy();
        transform.rotation =  Quaternion.Euler(new Vector3(0,0,0));
        destroy_splash.Emit(20);
        ambience.Stop(false, ParticleSystemStopBehavior.StopEmitting);
    }

}
