using UnityEngine;

public class RelocationDoor : Door{
    [Header("Relocation Door")]
    [SerializeField] string exit_point;
    public override void enter(){
        Player.instance.respawn_point = exit_point;
        Player.instance.door_enter_state();
        Player.instance.set_enter_position();
        close();
    }
}
