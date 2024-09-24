using UnityEngine;

public class Interactor : Collider2DTracker{
    [Header("Interactor")]
    [SerializeField] private GameObject parent_object;
    public void interact(){
        foreach(GameObject other in others)
            other.GetComponent<Interactable>().interact();
    }
}