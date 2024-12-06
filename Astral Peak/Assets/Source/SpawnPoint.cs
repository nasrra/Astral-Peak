using Unity.VisualScripting;
using UnityEngine;

public class SpawnPoint : MonoBehaviour{
    [SerializeField] MovementOption movement;
    void OnEnable() => SpawnPointManager.add_point(this);
    void OnDisable() => SpawnPointManager.erase_point(this);
    public MovementOption get_movement() => movement;
}