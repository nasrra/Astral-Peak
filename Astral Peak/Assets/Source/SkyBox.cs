using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyBox : MonoBehaviour{
    float start;
    [SerializeField] Transform cam;
    [SerializeField] float factor;
    void Awake() => start = transform.position.x;
    void LateUpdate(){
        float distance = cam.transform.position.x * factor;
        transform.position = new Vector3(start + distance, transform.position.y, transform.position.z);
    }
}
