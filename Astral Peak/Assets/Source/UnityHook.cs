using System;
using UnityEngine;

// this is a class used for static manager classes to hook into.
// for things such as coroutines and other unity engine related tasks.
public class UnityHook : MonoBehaviour{
    public static UnityHook instance;
    // public static AudioPlayer audio_player;
    public event Action
        start;
    void Awake(){
        instance = this;
        // audio_player = gameObject.AddComponent<AudioPlayer>();
    } 
    void Start() => start?.Invoke();
}
