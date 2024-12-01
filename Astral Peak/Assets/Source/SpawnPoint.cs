using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour{
    [SerializeField] SpawnPointType type;
    [SerializeField] MovementOption movement;
    void OnEnable() => SpawnPointManager.add_point(this);
    void OnDisable() => SpawnPointManager.erase_point(this);
    public SpawnPointType get_type() => type;
    public MovementOption get_movement() => movement;
}

public enum SpawnPointType{
    DOOR,
    RESPAWN,
}