using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour{
    [SerializeField] public UnityEvent unity_event;
    [SerializeField] private bool interactable = true;
    public void interact(){
        if(interactable)
            unity_event.Invoke();
    }
    public void is_interactable(int x) => interactable = x != 0;
}