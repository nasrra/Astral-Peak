using System;
using System.Collections;
using UnityEngine;

public class SimpleMovement : MonoBehaviour{
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] protected Coroutine 
        move_state,
        rotate_state;




    // state_machine
    protected void state_switch(ref Coroutine state, IEnumerator _state){
        if(state != null)
            StopCoroutine(state);
        state = _state != null ? StartCoroutine(_state) : null;
    }
    public void movement_state(Vector2 direction, float speed) => state_switch(ref move_state, movement(direction, speed));
    public void movement_state(Vector2 direction, float speed, float time) => state_switch(ref move_state, movement(direction, speed, time));
    public void move_to_target_state(Transform target, float speed) => state_switch(ref move_state, move_to_target(target,speed));
    public void rotate_to_target_state(Transform target, float speed) => state_switch(ref rotate_state, rotate_to_target(target, speed));
    public void rotate_to_target_loop_state(Transform target, float speed) => state_switch(ref rotate_state, rotate_to_target_loop(target, speed));
    public void rotate_to_direction_state(Vector2 direction, float speed) => state_switch(ref rotate_state, rotate_to_direction(direction, speed));
    public void stop_rotation_state() => state_switch(ref rotate_state, null);
    public void zero_velocity() => rb.linearVelocity = Vector3.zero;




    // movement.
    private IEnumerator movement(Vector2 direction, float speed){
        while(true){
            rb.linearVelocity = direction.normalized * speed;
            yield return new WaitForFixedUpdate();
        }
    }
    private IEnumerator movement(Vector2 direction, float speed, float time){
        float counter = time;
        while(time > 0){
            rb.linearVelocity = direction.normalized * speed;
            counter -= Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        yield break;       
    }
    private IEnumerator move_to_target(Transform target, float speed){
        while(true){
            rb.linearVelocity = (target.position - transform.position).normalized * speed;
            yield return new WaitForFixedUpdate();
        }
    }
    public void set_velocity(Vector2 velocity) => rb.linearVelocity = velocity;


    // rotation
    //private IEnumerator rotate_to_target(Transform target, float speed){
    //    while(true){
    //        Vector3 vec_to_target = target.position - transform.position;
    //        float angle = Mathf.Atan2(vec_to_target.y, vec_to_target.x) * Mathf.Rad2Deg;
    //        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
    //        transform.rotation = Quaternion.Slerp(transform.rotation, q, speed * Time.deltaTime);
    //        Debug.Log(transform.rotation);
    //        yield return new WaitForFixedUpdate();
    //    }
    //}
    //private IEnumerator rotate_to_target(Transform target, float speed, float time){
    //    float counter = time;
    //    while(counter > 0){
    //        Vector3 vec_to_target = target.position - transform.position;
    //        float angle = Mathf.Atan2(vec_to_target.y, vec_to_target.x) * Mathf.Rad2Deg;
    //        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
    //        transform.rotation = Quaternion.Slerp(transform.rotation, q, speed * Time.deltaTime);
    //        counter -= Time.deltaTime;
    //        yield return new WaitForFixedUpdate();
    //    }
    //    yield break;
    //}

    private IEnumerator rotate_to_target_loop(Transform target, float speed){
        while (true){
            Vector2 direction = (target.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, angle);
            Quaternion temp = Quaternion.Slerp(transform.rotation, targetRotation, speed * Time.deltaTime);
            // Update rotation smoothly with adjustments for axis consistency
            transform.rotation = Quaternion.Euler(0f, temp.eulerAngles.y, temp.eulerAngles.z);
            yield return new WaitForFixedUpdate();
        }
    }

    private IEnumerator rotate_to_target(Transform target, float speed){
        // Calculate the target angle based on the direction vector
        Vector2 direction = (target.position - transform.position).normalized;
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
        yield break;
    }
    private IEnumerator rotate_to_direction(Vector2 direction, float speed){
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
        yield break;
    }
}