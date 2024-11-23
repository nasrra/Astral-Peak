using UnityEngine;

public class ArenaBound : MonoBehaviour{
    [SerializeField] Collider2D col;
    
    void Start() => link();
    void OnDestroy() => unlink();

    void solid_collider() => col.isTrigger      = false;
    void trigger_collider() => col.isTrigger    = true;

    void link(){
        if(BossRoomHandler.instance != null){
            BossRoomHandler.instance.fight_started += solid_collider;
            BossRoomHandler.instance.fight_stopped += trigger_collider;
        }
    }
    void unlink(){
        if(BossRoomHandler.instance != null){
            BossRoomHandler.instance.fight_started -= solid_collider;
            BossRoomHandler.instance.fight_stopped -= trigger_collider;  
        }      
    }
}
