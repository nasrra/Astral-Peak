using System;
using UnityEngine;

public class FetchSword : Projectile{
    public event Action landed;
    [SerializeField] Animator animator;
    [SerializeField] TrailRenderer trail;
    [SerializeField] Collider2D col;

    protected override void OnTriggerEnter2D(Collider2D other){
        if(other.tag == "Player"){
            other.GetComponent<Player>().damage(1);
            other.GetComponent<CharacterMovement>().knockback(other.transform.position - transform.position, 20, 0.25f);
        }
        else{
            //check which way the sword is moving and flip accordingly.
            transform.rotation = rb.velocity.x >= 0? Quaternion.Euler(0,0,0) : Quaternion.Euler(0,180,0);
            animator.Play("landed");
            rb.velocity = Vector3.zero;
            rb.gravityScale = 0;
            landed?.Invoke();
        }
    }

    // used in animator.
    public void turn_trail_off() => trail.enabled = false;
    public void turn_collider_off() => col.enabled = false;
}
