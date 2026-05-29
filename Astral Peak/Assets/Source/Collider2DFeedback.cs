using System;
using UnityEngine;

public class Collider2DFeedback : MonoBehaviour{
    public event Action<Collider2D> trigger_enter;
    public event Action<Collider2D> trigger_exit;
    private void OnTriggerEnter2D(Collider2D other) => trigger_enter?.Invoke(other);
    private void OnTriggerExit2D(Collider2D other) => trigger_exit?.Invoke(other);       
    // TODO: add a function that sets all child objects parent to null, call it in the animator when this ground object is disabled. :)))
}