using Deluz;
using UnityEngine;

public class LightningStrikeProjectile : Projectile{
    [Header("Lightning Strike")]
    [SerializeField] LightningController lightning;
    [SerializeField] float lifetime, move_speed;

    public override void destroy(){
        Destroy(gameObject);
    }

    void Awake(){
        StartCoroutine(Util.timer(
            time: lifetime,
            time_out: destroy
        ));
        StartCoroutine(Util.timer(
            time: 1.5f,
            start_action:()=> enable_colliders(false),
            time_out: loop
        ));
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
        SceneLightining.instance.lerp_intensity(
            id:"global",
            value:2.5f,
            time:.15f,
            callback:()=>SceneLightining.instance.lerp_intensity(
                id: "global",
                value: 1,
                time:.15f));
        movement.movement_state(Player.instance.transform.position.x - transform.position.x <= 0? Vector2.left : Vector2.right, move_speed); 
    }
    
    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.layer == LayersManager.PLAYER)
            damage_creature(other.GetComponent<Creature>());
    }
}
