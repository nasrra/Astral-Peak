using UnityEngine;
using Entropek;
//
public class Arrow : Projectile{
    [Header("Arrow")]
    [SerializeField] ParticleSystem smoke, snow;
    [SerializeField] protected Transform front_point;
    [SerializeField] float move_time, rotation_speed, move_speed;
    void Awake(){
        play_sound();
    }    
    protected override void Start(){
        movement.move_to_target_state(front_point, move_speed);
        StartCoroutine(Util.timer(
            move_time, 
            time_out:()=>movement.rotate_to_direction_state(Vector2.down, rotation_speed))); 
        base.Start();
    }

    protected override void play_sound(){
        audio_player.play_diegetic_loop("fire_crackle_soft");
    }

    protected override void stop_sound(){
        audio_player.stop_diegetic_loop("fire_crackle_soft");
        audio_player.play_diegetic_one_shot("fire_extinguish");
    }

    protected override void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.layer == LayersManager.PLAYER){
            Creature creature = other.GetComponent<Creature>();
            creature.health.damaged += grounded;
            damage_creature_and_self_destruct(creature);
            creature.health.damaged -= grounded;
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
        audio_player.play_diegetic_one_shot("snow_impact_light");        
    }

    public override void destroy(){
        if(_is_being_destroyed == true)
            return;
        _is_being_destroyed = true;
        enable_sprites(false);
        enable_colliders(false);
        ParticleSystem.ShapeModule shape = smoke.shape;
        shape.rotation = Quaternion.Inverse(transform.rotation).eulerAngles; // inverse so it is always emits up.
        smoke.Play();
        stop_sound();
        Destroy(gameObject, smoke.GetComponent<ParticleSystem>().main.duration);    
    }
}//
