using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class CavalryOpeningCutscene : Cutscene{
    AudioSource source;
    CavalryBossRoom room = BossRoomHandler.instance as CavalryBossRoom;
    public override void begin(){
        room.phase_1(); // enable rider
        room.set_positions(); // reset positions.
        Player.player.enter_cutscene_state();
        TheRider.instance.enter_cutscene_state();
        CutsceneManager.set_coroutine(coroutine());
    }

    IEnumerator coroutine(){
        room.cutscene_arrow.fire_once();
        CameraController.instance.zoom_in_state(8,1);
        //CameraController.instance.move_down_state(2,1);
        CameraController.instance.regulate_in_bounds(false);
        yield return new WaitForSeconds(3);
        CameraController.instance.set_target(TheRider.instance.transform);
        yield return new WaitForSeconds(2);
        TheRider.instance.cutscene_yell_state();
        yield return new WaitForSeconds(3);
        CameraController.instance.set_target(Player.player.transform);
        CameraController.instance.reset_zoom_state(1);
        CameraController.instance.reset_offset_state(1);
        CameraController.instance.regulate_in_bounds(true);
        end();
        yield break;
    }

    public override void end(){
        AudioManager.play_music(room.song);
        Player.player.exit_cutscene_state();
        TheRider.instance.exit_cutscene_state();
    }
}

public class CavalryPhaseTransition : Cutscene{
    AudioSource source;
    CavalryBossRoom room = BossRoomHandler.instance as CavalryBossRoom;
    public override void begin() => CutsceneManager.set_coroutine(fade());

    IEnumerator fade(){
        fade_to_black();

        yield return new WaitForSeconds(1f); 
        room.phase_1();
        room.set_positions();
        TheRider.instance.enter_cutscene_state();
        Player.player.enter_cutscene_state();
        fade_from_black();
        
        yield return new WaitForSeconds(1f);
        CameraController.instance.set_target(TheRider.instance.transform);
        
        yield return new WaitForSeconds(1);
        TheRider.instance.cutscene_whistle_state();
        
        // start playing background wolf animation.
        yield return new WaitForSeconds(2f);
        room.background_wolf.SetActive(true);
        CameraController.instance.set_target(room.background_wolf.transform);
        CameraController.instance.regulate_in_bounds(false);
        
        yield return new WaitForSeconds(8.475f);
        room.background_wolf.SetActive(false);
        CameraController.instance.regulate_in_bounds(true);
        
        yield return new WaitForSeconds(0.25f);
        room.phase_2();
        room.set_positions();
        TheCavalry.instance.enter_cutscene_state();
        CameraController.instance.set_target(TheCavalry.instance.transform);
        
        yield return new WaitForSeconds(3f);
        CameraController.instance.set_target(Player.player.transform);
        end(); 
        yield break;
    }

    public override void end(){
        AudioManager.play_music(room.song);
        Player.player.exit_cutscene_state();
        TheCavalry.instance.exit_cutscene_state();
    }
}
