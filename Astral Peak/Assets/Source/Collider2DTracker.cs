using UnityEngine;
using System.Collections.Generic;

public class Collider2DTracker : MonoBehaviour{
    [SerializeField] private List<GameObject> others = new List<GameObject>();
    [SerializeField] private string find_tag = "none";
    void OnTriggerEnter2D(Collider2D other){
        if (other.gameObject.tag == find_tag)
            others.Add(other.gameObject);
    }
    void OnTriggerExit2D(Collider2D other){
        if (other.gameObject.tag == find_tag)
            others.Remove(other.gameObject);
    }
    public List<GameObject> get_tracked_objects(){
        return others;
    }
}