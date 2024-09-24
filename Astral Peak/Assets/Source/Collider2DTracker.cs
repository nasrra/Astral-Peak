using UnityEngine;
using System.Collections.Generic;

// need to swap this for the collider 2D feedback
// this is not a good idea to keep, just have the feedback give the list of 'others' to the interactor.

public class Collider2DTracker : MonoBehaviour{
    [Header("Collider 2D Tracker")]
    [SerializeField] protected bool colliding = false;
    [SerializeField] protected List<GameObject> others = new List<GameObject>();
    [SerializeField] protected string find_tag = "none";
    void OnTriggerEnter2D(Collider2D other){
        if (find_tag == "none" || other.gameObject.tag == find_tag){
            others.Add(other.gameObject);
            colliding = true;
        }
    }
    void OnTriggerExit2D(Collider2D other){
        if (find_tag == "none" || other.gameObject.tag == find_tag){
            others.Remove(other.gameObject);
            if(others.Count <= 0)
                colliding = false;
        }
    }
    public List<GameObject> get_tracked_objects(){
        return others;
    }
    public bool is_colliding(){
        return colliding;
    }
}