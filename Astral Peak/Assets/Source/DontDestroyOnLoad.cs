using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour{
    public event Action on_start;
    void Awake() => DontDestroyOnLoad(gameObject);
    void Start() => on_start?.Invoke();
}
