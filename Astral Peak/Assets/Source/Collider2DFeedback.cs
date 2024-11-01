using System;
using UnityEngine;

public class Collider2DFeedback : MonoBehaviour{
    public event Action<Collider2D> trigger_enter;
    public event Action<Collider2D> trigger_exit;
    protected virtual void OnTriggerEnter2D(Collider2D other){
        trigger_enter?.Invoke(other);
    }
    protected virtual void OnTriggerExit2D(Collider2D other){
        trigger_exit?.Invoke(other);       
    }
}