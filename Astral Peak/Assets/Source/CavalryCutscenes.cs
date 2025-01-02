using System.Collections;
using UnityEngine;

namespace Cutscenes{
public class CavalryOpening : Cutscene{
    CavalryBossRoom room = BossRoomHandler.instance as CavalryBossRoom;
    public override IEnumerator get_coroutine(){
        room.phase_1(); // enable rider
        room.set_positions(); // reset positions.

        room.cutscene_arrow.fire();
        CameraController.instance.lerp_zoom(8,2f);
        //CameraController.instance.move_down_state(2,1);
        CameraController.instance.regulate_in_bounds(false);
        yield return new WaitForSeconds(3);
        CameraController.instance.set_target(TheRider.instance.transform);
        yield return new WaitForSeconds(2);
        TheRider.instance.cutscene_yell();
        yield return new WaitForSeconds(3);
        CameraController.instance.set_target(Player.instance.transform);
        CameraController.instance.reset_zoom(time:2);
        CameraController.instance.reset_offset(time:2);
        CameraController.instance.regulate_in_bounds(true);

        //
        AudioManager.play_music(room.song);
        end();
        yield break;
    }
}

public class CavalryPhaseTransition : Cutscene{
    CavalryBossRoom room = BossRoomHandler.instance as CavalryBossRoom;
    public override IEnumerator get_coroutine(){
        fade_to_black();

        yield return new WaitForSeconds(2f); 
        room.phase_1();
        room.set_positions();
        TheRider.instance.flip_to_target();
        fade_from_black();
        
        yield return new WaitForSeconds(1f);
        CameraController.instance.set_target(TheRider.instance.transform);
        
        yield return new WaitForSeconds(1);
        TheRider.instance.cutscene_whistle();
        
        // start playing background wolf animation.
        yield return new WaitForSeconds(2f);
        room.background_wolf.SetActive(true);
        CameraController.instance.set_target(room.background_wolf.transform);
        CameraController.instance.regulate_in_bounds(false);
        
        yield return new WaitForSeconds(8.65f);
        room.background_wolf.SetActive(false);
        room.phase_2();
        TheCavalry.instance.sound.play_sound("slam_impact");
        CameraController.instance.regulate_in_bounds(true);
        room.set_positions();
        TheCavalry.instance.enter_cutscene_state();
        CameraController.instance.set_target(TheCavalry.instance.transform);
        
        yield return new WaitForSeconds(3f);
        CameraController.instance.set_target(Player.instance.transform);
        AudioManager.play_music(room.song);
        end(); 
        yield break;
    }
}
}

