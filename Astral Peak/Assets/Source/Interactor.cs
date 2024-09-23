using Unity.VisualScripting;
using UnityEngine;

public class Interactor : MonoBehaviour{
    [SerializeField] Collider2DTracker tracker;
    public void interact(){
        foreach(GameObject other in tracker.get_tracked_objects())
            other.GetComponent<Interactable>().interact();
    }
}