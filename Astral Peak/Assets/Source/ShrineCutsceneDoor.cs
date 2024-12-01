using UnityEngine;

public class ShrineCutsceneDoor : MonoBehaviour{
    [SerializeField] Door door;
    void Awake() => ShrineCutscene.open_shrine_door += door.open;
    void OnDestroy() => ShrineCutscene.open_shrine_door -= door.open;
}
