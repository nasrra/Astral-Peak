using System;
using System.Collections;
using UnityEngine;

public class TransformMovement : MonoBehaviour{

    Coroutine move_state;

    private void state_switch(IEnumerator _coroutine){
        if(move_state!=null)
            StopCoroutine(move_state);  
        move_state = _coroutine != null? StartCoroutine(_coroutine) : null;
    }

    public virtual void freeform_approach_to(Transform _target, float _speed, Action _callback = null){
        state_switch(freeform_approach_towards(_target, _speed, _callback));
    }
    
    protected IEnumerator freeform_approach_towards(Transform _target, float _time, Action _callback = null){
        float elapsed_time = 0;
        while(_target != null){
            Vector3 distance  = _target.position - transform.position;
            Vector3 direction = distance.normalized; 
            elapsed_time += Time.deltaTime;
            Vector3 point = Vector3.Lerp(transform.position, _target.position, elapsed_time / _time);
            if(Mathf.Abs(distance.magnitude) <= 0.15f){
                transform.position = _target.position;
                _callback?.Invoke();
            }   
            transform.position = _target.position;
            yield return new WaitForFixedUpdate();
        }
    }
}
