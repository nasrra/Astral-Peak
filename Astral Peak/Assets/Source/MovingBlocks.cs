using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatforms : MonoBehaviour{
    [SerializeField] List<Platform> platforms = new List<Platform>();
    [SerializeField] float rotation_speed;
    [SerializeField] float radius;
    void Awake(){
        StartCoroutine(circle());
        //StartCoroutine(test());
    }

    IEnumerator circle(){
        // Calculate the angular offset between blocks
        float angleStep = 360f / platforms.Count;
        while (true){
            for (int i = 0; i < platforms.Count; i++){
                float angle = (Time.time * rotation_speed + i * angleStep) * Mathf.Deg2Rad;
                float x = transform.position.x + Mathf.Cos(angle) * radius;
                float y = transform.position.z + Mathf.Sin(angle) * radius;
                platforms[i].transform.position = new Vector3(x,y, 0);
            }
            yield return null;
        }
    }

    IEnumerator figure_eight(){
        // Calculate the angular offset between blocks
        float angleStep = 360f / platforms.Count;
        while (true){
            for (int i = 0; i < platforms.Count; i++){
                float angle = (Time.time * rotation_speed + i * angleStep) * Mathf.Deg2Rad;
                float x = transform.position.x + Mathf.Cos(angle) * radius;
                float y = transform.position.z + Mathf.Sin(angle * 2) * radius /2;
                platforms[i].transform.position = new Vector3(x,y, 0);
            }
            yield return null;
        }
    }

    IEnumerator test(){
        while(true){
            yield return new WaitForSeconds(2);
            foreach(Platform platform in platforms)
                platform.disable();
            yield return new WaitForSeconds(2);
            foreach(Platform platform in platforms)
                platform.enable();                
            yield return null;
        }
    }
}
