using System.Collections;
using System;
using UnityEngine;
using Sounds;

public class Arrow : Projectile{
    [SerializeField] GameObject despawn_effect, grounded_effect;
    [SerializeField] GameObject sprite;
    AudioSource source;

    void Start() => state_switch(ref rotate_state, arrow_behaviour());

    protected override void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.layer == LayersManager.PLAYER){
            Creature creature = other.GetComponent<Creature>();
            creature.get_health().damaged += destroy_projectile;
            creature.get_health().damage(new DamageData(1), new KnockbackData(20, 0.25f, transform));
            creature.get_health().damaged -= destroy_projectile;
        }
        else if(other.gameObject.layer == LayersManager.GROUND)
            StartCoroutine(grounded_behaviour());
    }

    IEnumerator arrow_behaviour(){
        yield return new WaitForSeconds(lifetime/2f);
        state_switch(ref rotate_state, change_direction(Vector2.down, 4));
        yield break;
    }

    protected IEnumerator grounded_behaviour(){
        if(rotate_state != null)
            StopCoroutine(rotate_state);
        if(move_state != null)
            StopCoroutine(move_state);
        if(lifetime_state != null)
            StopCoroutine(lifetime_state);
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Static;
        col.enabled = false;
        AudioClipHandler.play(
            SoundID.SNOW_IMPACT_LIGHT,
            audio_player: this, 
            AudioSourceSettings.DIEGETIC);  

        GameObject particle = Instantiate(grounded_effect, front_point.position, despawn_effect.transform.rotation);
        Destroy(particle, particle.GetComponent<ParticleSystem>().main.duration);

        yield return new WaitForSeconds(1);
        particle = Instantiate(despawn_effect, front_point.position, despawn_effect.transform.rotation);
        float death_delay = particle.GetComponent<ParticleSystem>().main.duration;
        Destroy(particle, death_delay);
        sprite.SetActive(false);
        AudioClipHandler.play(
            SoundID.STEAM,
            audio_player: this, 
            AudioSourceSettings.DIEGETIC);  
        
        yield return new WaitForSeconds(death_delay);
        Destroy(gameObject);
        yield break;
    }
}
