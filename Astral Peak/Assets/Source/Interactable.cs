using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour{
    [SerializeField] private UnityEvent unityEvent;
    [SerializeField] private bool interactable = true;

    public void interact(){
        if(interactable)
            unityEvent.Invoke();
    }
}