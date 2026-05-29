using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour{
    float start;
    [SerializeField] Camera cam;
    [SerializeField] float factor;
    void Awake() => start = transform.position.x;
    void LateUpdate(){
        float distance = cam.transform.position.x * factor;
        transform.position = new Vector3(start + distance, transform.position.y, transform.position.z);
    }
}
