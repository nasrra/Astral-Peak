using System;
using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour{
    public Action 
        death_started,
        death_completed;
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] protected Transform front_point;
    [SerializeField] protected SpriteRenderer sprite;
    [SerializeField] protected float
        move_speed = 25,
        lifetime = 5,
        deathtime =0;
    [SerializeField] TrailRenderer trail;
    [SerializeField] protected Collider2D col;
    [SerializeField] protected Coroutine 
        move_state,
        rotate_state,
        lifetime_state;

    void Awake(){
        state_switch(ref move_state, move(move_speed));
        state_switch(ref lifetime_state, lifetime_counter()); 
    }

    protected void state_switch(ref Coroutine coroutine, IEnumerator state){
        if(coroutine != null)
            StopCoroutine(coroutine);
        if(state != null)
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

    public void destroy_projectile() => StartCoroutine(destory_projectile_coroutine());
    protected IEnumerator destory_projectile_coroutine(){
        sprite.enabled = false;
        state_switch(ref move_state, null);
        rb.linearVelocity = Vector2.zero;
        death_started?.Invoke();
        yield return new WaitForSeconds(deathtime);
        death_completed?.Invoke();
        Destroy(gameObject);        
    }

    protected virtual IEnumerator none(){yield break;}
    protected virtual IEnumerator move(float speed){
        while(true){
            rb.linearVelocity = (front_point.position - transform.position).normalized * speed;
            yield return new WaitForFixedUpdate();
        }
    }
    protected virtual IEnumerator move(float speed, float time){
        float counter = time;
        while(time > 0){
            rb.linearVelocity = (front_point.position - transform.position).normalized * speed;
            counter -= Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
    }
    protected virtual IEnumerator rotate_to_target(Transform target, float speed){
        while(true){
            Vector3 vec_to_target = target.position - transform.position;
            float angle = Mathf.Atan2(vec_to_target.y, vec_to_target.x) * Mathf.Rad2Deg;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, q, speed * Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }
    }
    protected virtual IEnumerator rotate_to_target(Transform target, float speed, float time){
        float counter = time;
        while(counter > 0){
            Vector3 vec_to_target = target.position - transform.position;
            float angle = Mathf.Atan2(vec_to_target.y, vec_to_target.x) * Mathf.Rad2Deg;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, q, speed * Time.deltaTime);
            counter -= Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
    }

    protected IEnumerator change_direction(Vector2 direction, float speed){
        // Calculate the target angle based on the direction vector
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        // Adjust for current y-axis rotation
        float y_factor = Mathf.Approximately(transform.rotation.eulerAngles.y, -180f) ? -1 : 1;
        // Target rotation with z-axis based on angle and y-factor
        Quaternion targetRotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, angle * y_factor);
        // Rotate smoothly until the angle difference is very small
        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.05f){
            Quaternion temp = Quaternion.Slerp(transform.rotation, targetRotation, speed * Time.deltaTime);
            // Update rotation smoothly with adjustments for axis consistency
            transform.rotation = Quaternion.Euler(0f, temp.eulerAngles.y, temp.eulerAngles.z);
            yield return new WaitForFixedUpdate();
        }
        // Snap to the exact target rotation at the end
        transform.rotation = targetRotation;
    }

    protected IEnumerator lifetime_counter(){
        yield return new WaitForSeconds(lifetime);
        Destroy(this.gameObject);
        yield break;
    }

    public void enable_trail(bool x) {if(trail != null)trail.enabled = x;}
    public void enable_collider(bool x) {if(col != null)col.enabled = x;}
}
