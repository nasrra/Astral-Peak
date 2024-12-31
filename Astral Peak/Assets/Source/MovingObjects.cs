using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObjects : MonoBehaviour{
    [SerializeField] List<GameObject> objects = new List<GameObject>();
    [SerializeField] MovingObjectsBehavior behaviour;
    [SerializeField] float rotation_speed;
    [SerializeField] float radius;
    void Awake(){
        switch(behaviour){
            case MovingObjectsBehavior.CIRCLE:
                StartCoroutine(circle());
                break;
            case MovingObjectsBehavior.FIGURE_EIGHT:
                StartCoroutine(figure_eight());
                break;
        }
    }

    IEnumerator circle(){
        // Calculate the angular offset between blocks
        float angleStep = 360f / objects.Count;
        while (true){
            for (int i = 0; i < objects.Count; i++){
                float angle = (Time.time * rotation_speed + i * angleStep) * Mathf.Deg2Rad;
                float x = transform.position.x + Mathf.Cos(angle) * radius;
                float y = transform.position.z + Mathf.Sin(angle) * radius;
                objects[i].transform.position = new Vector3(x,y, 0);
            }
            yield return null;
        }
    }

    IEnumerator figure_eight(){
        // Calculate the angular offset between blocks
        float angleStep = 360f / objects.Count;
        while (true){
            for (int i = 0; i < objects.Count; i++){
                float angle = (Time.time * rotation_speed + i * angleStep) * Mathf.Deg2Rad;
                float x = transform.position.x + Mathf.Cos(angle) * radius;
                float y = transform.position.z + Mathf.Sin(angle * 2) * radius /2;
                objects[i].transform.position = new Vector3(x,y, 0);
            }
            yield return null;
        }
    }
}

enum MovingObjectsBehavior{
    CIRCLE,
    FIGURE_EIGHT,
}