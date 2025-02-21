using System;
using UnityEngine;

public class AnimationEvent : MonoBehaviour{
    public event Action<string> animation_event; 
    public void invoke_event(string _event_name) => animation_event?.Invoke(_event_name);
    void OnDestroy(){
        // unlink all.
        animation_event = null;
    }
}
