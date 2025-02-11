using System.Collections;
using UnityEngine;

namespace Cutscenes{
public class DomineDoorOpening : Cutscene{
    GiantBossRoom room = BossRoomHandler.instance as GiantBossRoom;
    public override IEnumerator get_coroutine(){
        DomineDoor domine_door = room.get_domine_door();
        yield return new WaitForSeconds(2);
        domine_door.open();
        CameraController.instance.set_target(domine_door.transform);
        CameraController.instance.lerp_zoom(6,2);
        yield return new WaitForSeconds(4);
        CameraController.instance.reset_zoom(4);
        CameraController.instance.set_target(Player.instance.transform);
        yield return new WaitForSeconds(4);
        end();
        yield break;
    }
}

public class DomineDoorTransition1 : Cutscene{
    GiantBossRoom room = BossRoomHandler.instance as GiantBossRoom;
    public override IEnumerator get_coroutine(){
        DomineDoor domine_door = room.get_domine_door();
        CharacterMovement player_movement = Player.instance.get_movement() as CharacterMovement;
        player_movement.mod_gravity(0);
        player_movement.halt();
        Player.instance.transform.position = domine_door.get_player_point().position;
        Player.instance.get_sprite().fade_to_black();
        Player.instance.get_sprite().enter_domine_door_layer();
        yield return new WaitForSeconds(8);
        CameraController.instance.set_target(domine_door.transform);
        CustomSceneManager.load_scene_with_transitions("AstralPlane");
        CustomSceneManager.loaded_scene += play_transition_2;
        yield break;
    }
    protected void play_transition_2(){
        CustomSceneManager.loaded_scene -= play_transition_2;
        CutsceneManager.play(new AstralPlane());
    }
}

public class AstralPlane : Cutscene{
    public override IEnumerator get_coroutine(){
        Player.instance.get_sprite().set_black();
        CameraEffects.instance.astral_plane_state();
        yield return new WaitForSeconds(2);
        Player.instance.get_sprite().fade_from_black();
        yield return new WaitForSeconds(6);
        DialogueHandler.instance.play_dialogue(2.65f);
        DialogueHandler.instance.dialogue_ended += ending;
        yield break;
    }

    void ending(){
        DialogueHandler.instance.dialogue_ended -= ending;
        end();
    }
}
}
