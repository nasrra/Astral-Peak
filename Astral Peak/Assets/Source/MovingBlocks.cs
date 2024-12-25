using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBlocks : MonoBehaviour{
    [SerializeField] List<Transform> blocks = new List<Transform>();
    [SerializeField] float rotation_speed;
    [SerializeField] float radius;
    void Awake() => StartCoroutine(figure_eight());

    IEnumerator figure_eight(){

        while (true){
            // Calculate the angular offset between blocks
            float angleStep = 360f / blocks.Count;

            for (int i = 0; i < blocks.Count; i++){
                float angle = (Time.time * rotation_speed + i * angleStep) * Mathf.Deg2Rad;
                float x = transform.position.x + Mathf.Cos(angle) * radius;
                float y = transform.position.z + Mathf.Sin(angle) * radius;
                blocks[i].position = new Vector3(x,y, 0);
            }
            yield return null;
        }
    }
}
