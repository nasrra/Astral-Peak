using System;
using UnityEngine;

public class Collider2DFeedback : MonoBehaviour{
    public event Action<Collider2D> trigger_enter;
    public event Action<Collider2D> trigger_exit;
    [SerializeField] protected string find_tag = "none";
    protected virtual void OnTriggerEnter2D(Collider2D other){
        if(find_tag == "none" || other.gameObject.tag == find_tag)
            trigger_enter?.Invoke(other);
    }
    protected virtual void OnTriggerExit2D(Collider2D other){
        if(find_tag == "none" || other.gameObject.tag == find_tag)
            trigger_exit?.Invoke(other);       
    }
}