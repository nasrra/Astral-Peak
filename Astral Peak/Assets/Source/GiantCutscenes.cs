using System.Collections;
using UnityEngine;

namespace Cutscenes{
public class GiantOpening : Cutscene{
    GiantBossRoom room = BossRoomHandler.instance as GiantBossRoom;
    public override IEnumerator get_coroutine(){
        CameraController.instance.set_target(room.get_domine_door());
        CameraController.instance.lerp_zoom(6,2);
        yield return new WaitForSeconds(4);
        room.get_gateway_1().fade_in();
        room.get_gateway_2().fade_in();
        yield return new WaitForSeconds(8);
        room.get_gateway_1().fade_out();
        room.get_gateway_2().fade_out();
        CameraController.instance.reset_zoom(4);
        yield return new WaitForSeconds(4);
        room.play_giant1_introduction();
        yield return new WaitForSeconds(7);
        CameraController.instance.set_target(Player.instance.transform);
        yield return new WaitForSeconds(2);
        end();
        yield break;
    }
}

public class GiantPhaseTransition : Cutscene{
    public override IEnumerator get_coroutine(){
        Debug.Log("giant phase transition cutscene not implemented!");
        end();
        yield break;
    }
}

}
