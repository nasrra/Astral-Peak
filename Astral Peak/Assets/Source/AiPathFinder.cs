using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiPathFinder : MonoBehaviour{
    [SerializeField] private AiType type;
    [SerializeField] private List<AiPath> paths = new List<AiPath>();
    [SerializeField] private Movement movement;
    private Coroutine coroutine;
    void Start(){
        begin();
    }

    public void begin(){
        coroutine = StartCoroutine(loop());
    }

    public void stop(){
        if(coroutine != null)
            StopCoroutine(coroutine);
    }

    IEnumerator loop(){
        //while(true){
            // start movement.
            movement.movement(paths[0].get_movement(), true);
            yield return null;
        //}
    }
}

// Path that the Ai will follow.
[System.Serializable]
public struct AiPath{
    // direction to move in.
    [SerializeField] private MovementOption movement;
    // duration of movement.
    [SerializeField] private float duration;
    public MovementOption get_movement() => movement;
    public float get_duration() => duration; 
}

public enum AiType{
    AERIAL,
    GROUNDED,
    CHARACTER,
}
