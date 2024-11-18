using System;
using UnityEngine;

// this is a class used for static manager classes to hook into.
// for things such as coroutines and other unity engine related tasks.
public class UnityHook : MonoBehaviour{
    public static UnityHook instance;
    public event Action
        start;
    void Awake() => instance = this;
    void Start() => start?.Invoke();
}
