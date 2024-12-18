using System;
using UnityEngine;

public class AnimationEvent : MonoBehaviour{
    public event Action<string> signal;
    public void emit_animation_event_signal(string x) => signal?.Invoke(x);
}
