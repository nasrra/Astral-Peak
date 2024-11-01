using System;
using UnityEngine;

// get rid of tags, make everything use layers to fix 

public class FetchSword : Projectile{
    public event Action landed;
    [SerializeField] Animator animator;

    protected override void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.layer == 7){ // hits player
            other.GetComponent<Player>().damage(1);
            other.GetComponent<CharacterMovement>().knockback(other.transform.position - transform.position, 20, 0.25f);
        }
        else if(other.gameObject.layer == 6){ // hits ground
            //check which way the sword is moving and flip accordingly.
            transform.rotation = rb.velocity.x >= 0? Quaternion.Euler(0,0,0) : Quaternion.Euler(0,180,0);
            animator.Play("landed");
            rb.velocity = Vector3.zero;
            rb.gravityScale = 0;
            enable_trail(false);
            enable_collider(false);
            landed?.Invoke();
        }
    }
}
