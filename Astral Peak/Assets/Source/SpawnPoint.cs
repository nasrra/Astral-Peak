using Unity.VisualScripting;
using UnityEngine;
using System;

public class SpawnPoint : MonoBehaviour{
    [SerializeField] public event Action spawn_used;
    [SerializeField] MovementOption movement;
    void OnEnable() => SpawnPointManager.add_point(this);
    void OnDisable() => SpawnPointManager.erase_point(this);
    public MovementOption get_movement() => movement;
    public void use_spawn() => spawn_used?.Invoke();
}