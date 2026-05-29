using UnityEngine;

public class RespawnPoint : SpawnPoint{
    void OnTriggerEnter2D() => Player.instance.respawn_point = name;
}
