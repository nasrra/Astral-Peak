using UnityEngine;

public class DialogueEndedDoorLink : MonoBehaviour
{
    [SerializeField] Door door;
    void Start() => DialogueHandler.instance.dialogue_ended += door.open;
    void OnDestroy() => DialogueHandler.instance.dialogue_ended -= door.open;
}
