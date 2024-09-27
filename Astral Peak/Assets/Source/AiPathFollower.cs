using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AiPathFollower<T> : MonoBehaviour where T : Movement{
    [SerializeField] protected float path_timer = 0.0f;
    [SerializeField] protected int path_index = 0;
    [SerializeField] protected AiPath current_path;
    [SerializeField] protected List<AiPath> paths = new List<AiPath>();
    [SerializeField] private T movement;
    protected Coroutine coroutine;
    void Start() => begin();
    public void begin(){
        if(paths.Count > 0)
            coroutine = StartCoroutine(loop());
    }

    public void stop(){
        if(coroutine != null)
            StopCoroutine(coroutine);
    }

    protected IEnumerator loop(){
        while(true){
            // start movement.
            if(path_timer == 0.0f){
                current_path = paths[path_index];
                movement.movement(current_path.get_movement(), true);
                path_timer = current_path.get_duration();
            }
            // count down duration timer.
            else if(path_timer > 0.0f)
                path_timer -= Time.deltaTime;
            // stop movement.
            else{
                path_timer = 0.0f;
                movement.movement(current_path.get_movement(), false);
                path_index = ((path_index + 1) >= paths.Count)? 0 : path_index + 1;
            }
            yield return null;
        }
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
