using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace Cutscenes{
public class CavalryOpening : Cutscene{
    public override IEnumerator get_coroutine(){
        CavalryBossRoom room = BossRoomHandler.instance as CavalryBossRoom;
        Rider rider = room.get_rider();
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
        yield return new WaitForSeconds(5);
        CameraController.instance.set_target(Player.instance.transform);
        CameraController.instance.reset_zoom(time:2);
        CameraController.instance.reset_offset(time:2);
        CameraController.instance.regulate_in_bounds(true);

        //
        AudioManager.play_music(Sounds.SoundID.WOLF_BOSS_MUSIC_1);
        end();
        yield break;
    }
}

public class CavalryPhaseTransition : Cutscene{
    public override IEnumerator get_coroutine(){
        CavalryBossRoom room = BossRoomHandler.instance as CavalryBossRoom;
        Rider rider = room.get_rider();
        Cavalry cavalry = room.get_cavalary();
        CameraEffects.instance.fade_to_black(fade_transition_time);

        yield return new WaitForSeconds(2f); 
        cavalry.gameObject.SetActive(false);
        rider.gameObject.SetActive(true);
        rider.transform.position = room.get_boss_point(0).position;
        Player.instance.set_enter_position();
        rider.flip_to_target();
        CameraEffects.instance.fade_from_black(fade_transition_time);
        
        yield return new WaitForSeconds(1f);
        CameraController.instance.set_target(rider.transform);
        
        yield return new WaitForSeconds(1);
        rider.cutscene_whistle();
        
        // start playing background wolf animation.
        yield return new WaitForSeconds(2f);
        room.background_wolf.SetActive(true);
        CameraController.instance.set_target(room.background_wolf.transform);
        CameraController.instance.regulate_in_bounds(false);
        
        yield return new WaitForSeconds(8.65f);
        room.background_wolf.SetActive(false);
        cavalry.gameObject.SetActive(true);
        rider.gameObject.SetActive(false);
        cavalry.transform.position = room.get_boss_point(1).position;
        cavalry.sound.play_sound("slam_impact");
        CameraController.instance.regulate_in_bounds(true);
        cavalry.enter_cutscene_state();
        CameraController.instance.set_target(cavalry.transform);
        
        yield return new WaitForSeconds(3f);
        CameraController.instance.set_target(Player.instance.transform);
        AudioManager.play_music(Sounds.SoundID.WOLF_BOSS_MUSIC_2);
        end(); 
        yield break;
    }
}
}

