using UnityEngine;

public class TowerRoomHandler : RoomHandler{
    public override void game_cleared_room_state(){
        CameraController.instance.set_offset(new Vector3(0,40,-10), true);
        base.game_cleared_room_state();
    }

}
