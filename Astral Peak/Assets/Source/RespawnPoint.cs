public class RespawnPoint : SpawnPoint{
    void OnTriggerEnter2D() => Player.instance.set_respawn_point(name);
}
