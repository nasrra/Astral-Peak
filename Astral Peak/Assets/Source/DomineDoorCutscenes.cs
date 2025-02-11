using System;
using System.Collections;
using NUnit.Framework;
using UnityEngine;

namespace Cutscenes{
public class DomineDoorOpening : Cutscene{
    GiantBossRoom room = RoomHandler.instance as GiantBossRoom;
    public override IEnumerator get_coroutine(){
        Debug.Log(room);
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
    int current_level_pan_level = 0;
    string[] level_pan_levels = {
        "MageBossRoom",
        "SnowField",
        "WolfBossRoom",
        "SnowForest",
        "Tower",
        "TutorialRoom",
    };

    GiantBossRoom room = RoomHandler.instance as GiantBossRoom;
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
        CustomSceneManager.loaded_scene += level_pan;
        yield break;
    }
    protected void play_transition_2(){
        CustomSceneManager.loaded_scene -= play_transition_2;
        CutsceneManager.set_coroutine(astral_plane_opening());
    }
    IEnumerator astral_plane_opening(){
        Player.instance.get_sprite().set_black();
        CameraEffects.instance.astral_plane_state();
        yield return new WaitForSeconds(2);
        Player.instance.get_sprite().fade_from_black();
        yield return new WaitForSeconds(6);
        DialogueHandler.instance.play_dialogue(2.65f);
        DialogueHandler.instance.dialogue_ended += level_pan;
        yield break;        
    }

    void level_pan(){
        DialogueHandler.instance.dialogue_ended -= level_pan;
        CustomSceneManager.loaded_scene -= level_pan;
        pan_level();
    }

    void pan_level(){
        if(current_level_pan_level < level_pan_levels.Length){
            CustomSceneManager.loaded_scene += start_pan_level_loop;
            CustomSceneManager.load_scene_with_transitions(level_pan_levels[current_level_pan_level]);
            current_level_pan_level++;
        }
        else
            CutsceneManager.set_coroutine(ending());    
    }
    void start_pan_level_loop(){
        CustomSceneManager.loaded_scene -= start_pan_level_loop;
        CutsceneManager.set_coroutine(pan_level_loop());
    }

    IEnumerator pan_level_loop(){
        RoomHandler room = RoomHandler.instance;
        room.game_cleared_room_state();
        yield return new WaitForSeconds(8);
        pan_level();
        yield break;
    }

    IEnumerator ending(){
        CustomSceneManager.load_scene_with_transitions("AstralPlane");
        yield return new WaitForSeconds(6);
        end();
        yield break;
    }
}
}
