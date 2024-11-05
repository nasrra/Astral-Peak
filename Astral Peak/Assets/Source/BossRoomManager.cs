using UnityEngine;

public class BossRoomManager : MonoBehaviour{
    // play the opening cutscene to a boss room.
    void Start()=>CutsceneManager.instance.Play("test");
}
