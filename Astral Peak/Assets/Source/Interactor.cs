using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Interactor : MonoBehaviour{
    [Header("Interactor")]
    [SerializeField] private Collider2DFeedback feedback;
    [SerializeField] private List<GameObject> others = new List<GameObject>();

    void Start(){
        link_events();
    }

    void OnDestroy(){
        unlink_events();
    }
    
    public void interact(){
        foreach(GameObject other in others)
            other.GetComponent<Interactable>().interact();
    }

    private void interactable_in_range(Collider2D other){
        others.Add(other.gameObject);
    }
    
    private void interactable_left_range(Collider2D other){
        others.Remove(other.gameObject);
    }

    private void link_events(){
        feedback.trigger_enter += interactable_in_range;
        feedback.trigger_exit += interactable_left_range;
    }

    private void unlink_events(){
        feedback.trigger_enter -= interactable_in_range;
        feedback.trigger_exit -= interactable_left_range;
    }
}