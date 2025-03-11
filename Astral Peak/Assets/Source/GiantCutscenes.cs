using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace Cutscenes{
public class GiantOpening : Cutscene{
    GiantBossRoom room = BossRoomHandler.instance as GiantBossRoom;
    public override IEnumerator get_coroutine(){
        DomineDoor domine_door = room.get_domine_door();
        CameraController.instance.set_target(domine_door.transform);
        CameraController.instance.lerp_zoom(6,2);
        AudioManager.play_music("music_domine_door");
        yield return new WaitForSeconds(2);
        room.get_gateway_1().fade_in();
        room.get_gateway_2().fade_in();
        yield return new WaitForSeconds(8);
        room.get_gateway_1().fade_out();
        room.get_gateway_2().fade_out();
        CameraController.instance.reset_zoom(4);
        AudioManager.stop_music();
        yield return new WaitForSeconds(4);
        room.giant1.gameObject.SetActive(true);
        room.giant1.animator.Play("Giant1Intro");
        CameraController.instance.set_target(room.giant1.gameObject.transform);
        yield return new WaitForSeconds(1.5f);
        room.giant1.animator.Play("Giant1Yell");
        yield return new WaitForSeconds(4);
        stop_skip();
        room.giant1.particles.stop_particle("yell");
        CameraController.instance.set_target(Player.instance.transform);
        yield return new WaitForSeconds(1);
        AudioManager.play_music("music_the_giant_1");
        end();
        yield break;
    }
}
}
