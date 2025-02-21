using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace Cutscenes{
public class CavalryOpening : Cutscene{
    public override IEnumerator get_coroutine(){
        CavalryBossRoom room = RoomHandler.instance as CavalryBossRoom;
        Rider rider = room.rider;
        rider.gameObject.SetActive(true);
        rider.transform.position = room.get_boss_point(0).position;
        Player.instance.set_enter_position();
        room.cutscene_arrow.fire();
        CameraController.instance.lerp_zoom(6,2f);
        CameraController.instance.lerp_offset(3,-2,1);
        yield return new WaitForSeconds(3);
        CameraController.instance.lerp_offset(0,-2,1);
        CameraController.instance.set_target(rider.transform);
        yield return new WaitForSeconds(2);
        rider.cutscene_yell();
        yield return new WaitForSeconds(.5f);
        CameraController.instance.lerp_offset(0,4,3);
        CameraController.instance.lerp_zoom(14,5f);
        yield return new WaitForSeconds(4);
        stop_skip();
        yield return new WaitForSeconds(1);
        CameraController.instance.set_target(Player.instance.transform);
        CameraController.instance.reset_zoom(time:2);
        CameraController.instance.reset_offset(time:2);
        CameraController.instance.regulate_in_bounds(true);

        //
        // AudioManager.play_music(Sounds.SoundID.WOLF_BOSS_MUSIC_1);
        end();
        yield break;
    }
}

public class CavalryPhaseTransition : Cutscene{
    public override IEnumerator get_coroutine(){
        CavalryBossRoom room = RoomHandler.instance as CavalryBossRoom;
        Rider rider = room.rider;
        Cavalry cavalry = room.cavalry;
        CameraEffects.instance.fade_to_black(fade_transition_time);

        yield return new WaitForSeconds(2f); 
        cavalry.gameObject.SetActive(false);
        rider.gameObject.SetActive(true);
        rider.transform.position = room.get_boss_point(0).position;
        Player.instance.set_enter_position();
        rider.flip_to_target();
        CameraEffects.instance.fade_from_black(fade_transition_time);
        CameraController.instance.lerp_zoom(6,4);
        CameraController.instance.lerp_offset(null, -1 ,4);

        yield return new WaitForSeconds(2f);
        CameraController.instance.set_target(rider.transform);

        yield return new WaitForSeconds(1);
        rider.animator.Play("RiderPhaseTransition");
        yield return new WaitForSeconds(2.5f);
        CameraController.instance.lerp_offset(null, 45, 2);

        // start playing background wolf animation.
        yield return new WaitForSeconds(2f);
        room.background_wolf.SetActive(true);
        yield return new WaitForSeconds(.25f);
        CameraController.instance.set_target(room.background_wolf.transform);
        CameraController.instance.reset_zoom(2);
        CameraController.instance.reset_offset(2);
        yield return new WaitForSeconds(8.4f);
        room.background_wolf.SetActive(false);
        cavalry.gameObject.SetActive(true);
        rider.gameObject.SetActive(false);
        cavalry.transform.position = room.get_boss_point(1).position;
        CameraController.instance.regulate_in_bounds(true);
        cavalry.enter_cutscene_state();
        CameraController.instance.set_target(cavalry.transform);
        
        yield return new WaitForSeconds(2f);
        stop_skip();
        yield return new WaitForSeconds(1f);
        CameraController.instance.reset_offset(2);
        CameraController.instance.reset_zoom(2);
        CameraController.instance.set_target(Player.instance.transform);
        // AudioManager.play_music(Sounds.SoundID.WOLF_BOSS_MUSIC_2);
        end(); 
        yield break;
    }
}
}

