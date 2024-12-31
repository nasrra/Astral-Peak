using Deluz;
using UnityEngine;

public class LightningStrikeProjectile : Projectile{
    [SerializeField] LightningController lightning;
    void Awake(){
        state_switch(ref lifetime_state, lifetime_counter());
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
        state_switch(ref move_state, move(Player.instance.transform.position.x - transform.position.x <= 0? Vector2.left : Vector2.right)); 
    }
    protected override void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.layer == LayersManager.PLAYER){
            Creature creature = other.GetComponent<Creature>();
            creature.get_health().damage(new DamageData(1), new KnockbackData(20, 0.25f, transform));
        }
    }
}
