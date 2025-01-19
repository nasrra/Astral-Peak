using Entropek;
using UnityEngine;

public class LightningStrikeProjectile : Projectile{
    [Header("Lightning Strike")]
    [SerializeField] LineParticleEmitter lightning;
    [SerializeField] float lifetime, move_speed;

    public override void destroy(){
        Destroy(gameObject);
    }

    void Awake(){
        enable_colliders(false);
    }

    protected override void Start(){
        StartCoroutine(Util.timer(
            time: lifetime,
            time_out: destroy
        ));
        StartCoroutine(Util.timer(
            time: 1.5f,
            time_out: loop
        ));
        snap_to_floor();
        base.Start();
    }
    void loop(){
        enable_colliders(true);
        lightning.start_emitting();
        AudioClipHandler.play(
            sound_id: Sounds.SoundID.THUNDER_1,
            audio_player: this,
            AudioSourceSettings.DIEGETIC_RANDOMISED
        );
        AudioClipHandler.play(
            sound_id: Sounds.SoundID.ELECTRICITY_LOOP,
            audio_player: this,
            AudioSourceSettings.DIEGETIC_RANDOMISED_LOOP
        );
        SceneLighting.instance.set_intensity(id:"global",value:3f);
        SceneLighting.instance.reset_lighting(_id: "global",_time:.2f);
        movement.movement_state(Player.instance.transform.position.x - transform.position.x <= 0? Vector2.left : Vector2.right, move_speed); 
    }
    
    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.layer == LayersManager.PLAYER)
            damage_creature(other.GetComponent<Creature>());
    }
}
