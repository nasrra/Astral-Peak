using System;
using UnityEngine;

public class SceneInfo : MonoBehaviour{
    public static SceneInfo instance;
    // should the scene be saved.
    [field: SerializeField] public bool saveable {get; private set;}
    // is the scene a transition between to level types.
    [field: SerializeField] public bool transition {get; private set;}
    // what type of scene this is.
    //[field: SerializeField] public SceneType scene_type {get; private set;}
    void Awake(){
        instance = this;
    }
}

[Serializable]
public enum SceneType{
    NONE,    
    SNOW,
    SHRINE,
    MENU,
}
