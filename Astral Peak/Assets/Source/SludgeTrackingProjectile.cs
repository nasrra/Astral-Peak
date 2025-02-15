using UnityEngine;
using Entropek;

public class SludgeTrackingProjectile : MagicStationaryProjectile{
    [Header("MagicTrackingProjectile")]
    [SerializeField] ParticleSystem destroy_splash;
    [SerializeField] Transform front_point, top_point;
    [SerializeField] Vector2 buffer_direction;
    [SerializeField] float rotate_speed, move_speed, buffer_time, buffer_speed;
    protected override void Start(){
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
        // source = AudioClipHandler.play(Sounds.SoundID.WATER_GURGLE_LOOP, gameObject, AudioSourceSettings.DIEGETIC_RANDOMISED_LOOP);
    } 
    
    protected override void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.layer == LayersManager.GROUND)
            destroy();
        base.OnTriggerEnter2D(other);
    }
    public override void destroy(){
        // AudioClipHandler.play(Sounds.SoundID.WATER_SPLASH, gameObject, AudioSourceSettings.DIEGETIC_RANDOMISED);
        ParticleSystem.ShapeModule shape = destroy_splash.shape;
        transform.rotation =  Quaternion.Euler(new Vector3(0,0,0));
        destroy_splash.Emit(20);
        movement.StopAllCoroutines();
        movement.zero_velocity();
        base.destroy();
    }
}
