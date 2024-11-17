using System;
using UnityEngine;

// this is a class used for static manager classes to hook into.
// for things such as coroutines and other unity engine related tasks.
public class UnityHook : MonoBehaviour{
    public event Action
        start;
    void Start() => start?.Invoke();
}
