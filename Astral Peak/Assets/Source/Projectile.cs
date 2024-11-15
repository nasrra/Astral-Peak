using System;
using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour{
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] protected Transform front_point;
    [SerializeField] protected float
        move_speed = 25,
        lifetime = 5;
    [SerializeField] TrailRenderer trail;
    [SerializeField] Collider2D col;
    [SerializeField] protected Coroutine 
        move_state,
        rotate_state;

    void Awake(){
        state_switch(move_state, move(move_speed));
        StartCoroutine(lifetime_counter());
    }

    protected void state_switch(Coroutine coroutine, IEnumerator state){
        if(coroutine != null)
            StopCoroutine(coroutine);
        coroutine = StartCoroutine(state);
    }

    protected virtual void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.layer == LayersManager.PLAYER){
            Creature creature = other.GetComponent<Creature>();
            creature.get_health().damaged += destroy_projectile;
            creature.get_health().damage(new DamageData(1), new KnockbackData(20, 0.25f, transform));
            creature.get_health().damaged -= destroy_projectile;
        }
        else if(other.gameObject.layer == LayersManager.GROUND)
            destroy_projectile();
    }

    public void destroy_projectile(){
        enable_trail(false);
        Destroy(gameObject);        
    }

    protected virtual IEnumerator none(){yield break;}
    protected virtual IEnumerator move(float speed){
        while(true){
            rb.velocity = (front_point.position - transform.position).normalized * speed;
            yield return new WaitForFixedUpdate();
        }
    }
    protected virtual IEnumerator move(float speed, float time){
        float counter = time;
        while(time > 0){
            rb.velocity = (front_point.position - transform.position).normalized * speed;
            counter -= Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
    }
    protected virtual IEnumerator rotate_to_target(float speed){
        while(true){
            Vector3 vec_to_target = Player.player.transform.position - transform.position;
            float angle = Mathf.Atan2(vec_to_target.y, vec_to_target.x) * Mathf.Rad2Deg;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, q, speed * Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }
    }
    protected virtual IEnumerator rotate_to_target(float speed, float time){
        float counter = time;
        while(counter > 0){
            Vector3 vec_to_target = Player.player.transform.position - transform.position;
            float angle = Mathf.Atan2(vec_to_target.y, vec_to_target.x) * Mathf.Rad2Deg;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, q, speed * Time.deltaTime);
            counter -= Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
    }

    protected IEnumerator lifetime_counter(){
        yield return new WaitForSeconds(lifetime);
        Destroy(this.gameObject);
        yield break;
    }

    public void enable_trail(bool x) {if(trail != null)trail.enabled = x;}
    public void enable_collider(bool x) {if(col != null)col.enabled = x;}
}
