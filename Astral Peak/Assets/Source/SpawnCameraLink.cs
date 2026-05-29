using System;
using UnityEngine;

public class SpawnCameraLink : MonoBehaviour{
    [SerializeField] SpawnPoint spawn;
    [SerializeField] Vector3 exit_offset;
    [SerializeField] float speed;
    void set_offset() => CameraController.instance.set_offset(exit_offset, true);
    void Awake() => spawn.spawn_used += set_offset;
    void OnDestroy() => spawn.spawn_used -= set_offset;
}
//