using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingProjectile : Projectile{
    [SerializeField] float
        buffer_time,
        snap_rotate_speed,
        lerp_rotate_speed,
        modifier;

    private bool flag = true;

    Coroutine
        rotation_coroutine;

    void Awake(){}
    void Start(){
        rotation_coroutine = StartCoroutine(rotate_to_target(snap_rotate_speed));
        StartCoroutine(buffer_timer());
    }

    protected override void move() => StartCoroutine(movement());
    protected override void stop() => StopCoroutine(movement());

    IEnumerator movement(){
        while(true){
            rb.velocity = (front_point.position - transform.position).normalized * speed;
            yield return new WaitForFixedUpdate();
        }
    }

    // snap rotation to face target.
    IEnumerator rotate_to_target(float speed){
        while(flag == true){
            Vector3 vec_to_target = Player.player.transform.position - transform.position;
            float angle = Mathf.Atan2(vec_to_target.y, vec_to_target.x) * Mathf.Rad2Deg - modifier;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, q, speed * Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator buffer_timer(){
        yield return new WaitForSeconds(buffer_time);
        StopAllCoroutines();
        move();
        rotation_coroutine = StartCoroutine(rotate_to_target(lerp_rotate_speed));
        enable_trail(true);
        yield break;
    }
}
