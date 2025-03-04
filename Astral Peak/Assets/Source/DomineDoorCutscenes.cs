using System;
using System.Collections;
using NUnit.Framework;
using UnityEngine;

namespace Cutscenes{
public class DomineDoorOpening : Cutscene{
    GiantBossRoom room = RoomHandler.instance as GiantBossRoom;
    public override IEnumerator get_coroutine(){
        DomineDoor domine_door = room.get_domine_door();
        yield return new WaitForSeconds(2);
        domine_door.open();
        CameraController.instance.set_target(domine_door.transform);
        CameraController.instance.lerp_zoom(6,2);
        yield return new WaitForSeconds(4);
        CameraController.instance.reset_zoom(4);
        CameraController.instance.set_target(Player.instance.transform);
        yield return new WaitForSeconds(3);
        stop_skip();
        yield return new WaitForSeconds(1);
        end();
        yield break;
    }
}

public class DomineDoorFinal : Cutscene{
    public Action torches_on, numerals_on;
    int current_level_pan_level = 0;
    string[] level_pan_levels = {
        "ShrineInterlude2",
        "SnowInterlude3",
        "MageBossRoom",
        "SnowInterlude2",
        "CavalryBossRoom",
        "SnowInterlude1",
        "ShrineInterlude1",
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
        yield return new WaitForSeconds(2); // 8
        CameraController.instance.set_target(domine_door.transform);
        CustomSceneManager.load_scene_with_transitions("AstralPlane");
        // CustomSceneManager.loaded_scene += astral_plane_segment;
        CustomSceneManager.loaded_scene += start_scene_swap_segment;
        //CustomSceneManager.loaded_scene += start_shrine_segment;
        //CustomSceneManager.loaded_scene += start_end_credits_segment;
        room.game_cleared_room_state();
        yield break;
    }
    protected void astral_plane_segment(){
        AudioManager.load_bank("cutscene_astral_plane");
        CustomSceneManager.loaded_scene -= astral_plane_segment;
        CutsceneManager.set_coroutine(astral_plane_opening());
    }
    IEnumerator astral_plane_opening(){
        Player.instance.get_sprite().set_black();
        AstralPlaneRoomHandler astral_room = RoomHandler.instance as AstralPlaneRoomHandler;
        astral_room.get_domine_door().opened();
        yield return new WaitForSeconds(2);
        DialogueHandler.instance.set_dialogue(ExcelReader.read_file("Dialogue", "DomineAstralPlane"));
        Player.instance.get_sprite().fade_from_black();
        yield return new WaitForSeconds(6);
        DialogueHandler.instance.play_dialogue(2.65f);
        DialogueHandler.instance.dialogue_ended += start_scene_swap_segment;
        yield break;        
    }

    void start_scene_swap_segment(){
        // AudioManager.lock_ambience  = true;
        AudioManager.lock_music     = true;
        AudioManager.unload_bank("cutscene_astral_plane");
        DialogueHandler.instance.dialogue_ended -= start_scene_swap_segment;
        CustomSceneManager.loaded_scene         -= start_scene_swap_segment;
        scene_swap_logic();
    }

    void scene_swap_logic(){
        if(current_level_pan_level < level_pan_levels.Length){
            CustomSceneManager.loaded_scene += start_scene_swap_coroutine;
            CustomSceneManager.load_scene_with_transitions(level_pan_levels[current_level_pan_level]);
            current_level_pan_level++;
        }
        else
            start_shrine_segment();   
    }
    void start_scene_swap_coroutine(){
        CustomSceneManager.loaded_scene -= start_scene_swap_coroutine;
        CutsceneManager.set_coroutine(scene_swap_coroutine());
    }

    IEnumerator scene_swap_coroutine(){
        RoomHandler room = RoomHandler.instance;
        room.game_cleared_room_state();
        yield return new WaitForSeconds(8);
        scene_swap_logic();
        yield break;
    }

    void start_shrine_segment(){
        AudioManager.lock_ambience  = false;
        AudioManager.lock_music     = false;
        CustomSceneManager.loaded_scene -= start_shrine_segment;
        DialogueHandler.instance.dialogue_ended -= start_shrine_segment;
        CustomSceneManager.load_scene_with_transitions("Shrine");
        CustomSceneManager.loaded_scene += start_shrine_segment_coroutine;
    }

    void start_shrine_segment_coroutine(){
        CustomSceneManager.loaded_scene -= start_shrine_segment_coroutine;
        CutsceneManager.set_coroutine(shrine_segement_coroutine());
    }

    IEnumerator shrine_segement_coroutine(){
        ShrineRoomHandler shrine_room = RoomHandler.instance as ShrineRoomHandler;
        shrine_room.game_cleared_room_state();
        numerals_on?.Invoke();
        yield return new WaitForSeconds(3);
        torches_on?.Invoke();
        yield return new WaitForSeconds(3);
        CameraController.instance.lerp_zoom(3,6);
        CameraController.instance.lerp_offset(null,-2.5f,6);
        yield return new WaitForSeconds(8);
        shrine_room.beatrice.awaken();
        yield return new WaitForSeconds(8.9f);
        // AudioManager.mute_sfx_volume();
        // AudioClipHandler.play(Sounds.SoundID.WOMAN_GASP_REVERB, UnityHook.instance.gameObject, AudioSourceSettings.NON_DIEGETIC);
        CameraEffects.instance.fade_to_black(0.01f);
        yield return new WaitForSeconds(8f);
        start_end_credits_segment();
        yield break;
    }

    void start_end_credits_segment(){
        CustomSceneManager.loaded_scene -= start_end_credits_segment;
        DialogueHandler.instance.dialogue_ended -= start_end_credits_segment;
        CustomSceneManager.load_scene("AstralPlane");
        CustomSceneManager.loaded_scene += ending;
    }

    void ending(){
        CustomSceneManager.loaded_scene -= ending;
        DialogueHandler.instance.dialogue_ended -= ending;
        AstralPlaneRoomHandler astral_room = RoomHandler.instance as AstralPlaneRoomHandler;
        CameraEffects.instance.fade_from_black(4);
        astral_room.end_credits_state();
        unlink();
        stop_skip();
        end();
    }

    void unlink(){
        torches_on = null;
        numerals_on = null;
    }
}
}
