using UnityEngine;
using Sounds;
using Entropek;

public class Arrow : Projectile{
    [Header("Arrow")]
    [SerializeField] ParticleSystem smoke, snow;
    [SerializeField] protected Transform front_point;
    [SerializeField] float move_time, rotation_speed, move_speed;
    protected override void Start(){
       movement.move_to_target_state(front_point, move_speed);
        StartCoroutine(Util.timer(
            move_time, 
            time_out:()=>movement.rotate_to_direction_state(Vector2.down, rotation_speed))); 
        base.Start();
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.layer == LayersManager.PLAYER){
            Creature creature = other.GetComponent<Creature>();
            creature.get_health().damaged += grounded;
            damage_creature_and_self_destruct(creature);
            creature.get_health().damaged -= grounded;
        }   
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
        snow.Play();
        ParticleSystem.ShapeModule shape = snow.shape;
        shape.rotation = Quaternion.Inverse(transform.rotation).eulerAngles; // inverse so it is always emits up.
        AudioClipHandler.play(
            SoundID.SNOW_IMPACT_LIGHT,
            audio_player: this, 
            AudioSourceSettings.DIEGETIC);        
    }

    public override void destroy(){
        enable_sprites(false);
        enable_colliders(false);
        ParticleSystem.ShapeModule shape = smoke.shape;
        shape.rotation = Quaternion.Inverse(transform.rotation).eulerAngles; // inverse so it is always emits up.
        smoke.Play();
        AudioClipHandler.play(
            SoundID.STEAM,
            audio_player: this, 
            AudioSourceSettings.DIEGETIC);
        Destroy(gameObject, smoke.GetComponent<ParticleSystem>().main.duration);    
    }
}
