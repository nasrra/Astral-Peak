using UnityEngine;

public class ArenaBound : MonoBehaviour{
    [SerializeField] Collider2D col;
    
    //void Start() => link();
    //void OnDestroy() => unlink();

    void solid_collider() => col.isTrigger      = false;
    void trigger_collider() => col.isTrigger    = true;

    //void link(){
    //    BossRoomHandler room = RoomHandler.instance as BossRoomHandler;
    //    room.fight_started += solid_collider;
    //    room.fight_stopped += trigger_collider;
    //}
    //void unlink(){
    //    BossRoomHandler room = RoomHandler.instance as BossRoomHandler;
    //    room.fight_started -= solid_collider;
    //    room.fight_stopped -= trigger_collider;  
    //}
}
