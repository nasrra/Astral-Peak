using Unity.VisualScripting;
using UnityEngine;

public class Interactor : Collider2DTracker{
    public void interact(){
        foreach(GameObject other in others)
            other.GetComponent<Interactable>().interact();
    }
}