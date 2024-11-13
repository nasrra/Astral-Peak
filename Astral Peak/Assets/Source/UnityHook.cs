using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// this is a class used for static manager classes to hook into.
// for things such as coroutines and other unity engine related tasks.
public class UnityHook : MonoBehaviour{
    public static UnityHook instance;
    void Awake(){
        if(instance == null){
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }
}
