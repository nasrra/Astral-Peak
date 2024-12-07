using System;
using System.Collections;
using System.Collections.Generic;
using DocumentFormat.OpenXml.Wordprocessing;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShrineOpeningCutscene : Cutscene{
    public event Action 
        open_shrine_door, 
        torches_on, 
        torches_off, 
        enlargen_torches, 
        reset_torches,
        world_constellation_on, 
        world_constellation_off, 
        gateway_constellation_on, 
        gateway_constellation_off, 
        aether_constellation_on, 
        aether_constellation_off, 
        soul_constellation_on, 
        soul_constellation_off;

    public override void start(){
        Player.instance.enter_cutscene_state();
        AudioManager.play_music(SoundID.DOMINE_THEME);
        DialogueHandler.instance.dialogue_ended += dialogue_ended;
        DialogueHandler.instance.new_line += handle_new_line;
        CutsceneManager.set_coroutine(starting_coroutine());
    }

    IEnumerator starting_coroutine(){
        torches_on?.Invoke();
        CameraController.instance.zoom_in_state(4.45f, .5f);
        CameraController.instance.move_vertical_state(2.2f, .5f);
        yield return new WaitForSeconds(5);
        DialogueHandler.instance.play_dialogue(3f);
        yield return new WaitForSeconds(3f);
        CameraController.instance.move_vertical_state(-2.5f, .2f);
        yield return new WaitForSeconds(40);
        CameraController.instance.reset_zoom_state(.2f);
        CameraController.instance.move_vertical_state(0, .2f);     
        yield break;
    }

    IEnumerator middle_coroutine(){
        CameraController.instance.zoom_in_state(2.5f, .5f);
        CameraController.instance.move_horizontal_state(-3.25f, .4f);
        yield return new WaitForSeconds(9);
        CameraController.instance.move_horizontal_state(3.25f, .3f);
        yield return new WaitForSeconds(26);
        CameraController.instance.reset_zoom_state(.33f);
        CameraController.instance.move_horizontal_state(0.05f, .4f);
        yield break;
    }

    public void dialogue_ended() => CutsceneManager.set_coroutine(ending_couroutine());

    IEnumerator ending_couroutine(){
        torches_off?.Invoke();     
        yield return new WaitForSeconds(5);  
        Player.instance.exit_cutscene_state();
        AudioManager.stop_music();
        unlink();
        end();
    }
    void handle_new_line(int line){
        switch(line){
            case 27: 
                open_shrine_door?.Invoke(); 
                break;
            case 10: 
                enlargen_torches?.Invoke();
                break;
            case 11: 
                reset_torches?.Invoke(); 
                torches_off?.Invoke(); 
                break;
            case 12: 
                reset_torches?.Invoke(); 
                torches_on?.Invoke(); 
                break;
            case 16: 
                CutsceneManager.set_coroutine(middle_coroutine()); 
                break;
            case 17: 
                world_constellation_on?.Invoke(); 
                break;
            case 18: 
                world_constellation_off?.Invoke(); 
                gateway_constellation_on?.Invoke();
                break;
            case 20:
                gateway_constellation_off?.Invoke();
                aether_constellation_on?.Invoke();
                break;
            case 21:
                aether_constellation_off?.Invoke();
                soul_constellation_on?.Invoke();
                break;
            case 22:
                soul_constellation_off?.Invoke();
                break;

        }
    }

    void unlink(){
        open_shrine_door            = null; 
        torches_on                  = null;
        torches_off                 = null;
        enlargen_torches            = null;
        reset_torches               = null;
        world_constellation_on      = null; 
        world_constellation_off     = null; 
        gateway_constellation_on    = null; 
        gateway_constellation_off   = null; 
        aether_constellation_on     = null; 
        aether_constellation_off    = null;
        soul_constellation_on       = null; 
        soul_constellation_off      = null;        
    }
}

public class ShrineAltarOneCutscene : Cutscene{
    public event Action
        torches_on, altar_numeral_on;

    public override void start() => CutsceneManager.set_coroutine(cutscene());

    IEnumerator cutscene(){
        AudioManager.play_music(SoundID.ALTAR_THEME);
        AudioManager.restore_sfx_smooth();
        Player.instance.gameObject.SetActive(false);
        CameraEffects.instance.flashback_state();
        
        yield return new WaitForSeconds(2f);
        torches_on?.Invoke();
        
        yield return new WaitForSeconds(7);
        altar_numeral_on?.Invoke();
        
        yield return new WaitForSeconds(7);
        AudioManager.stop_music();
        CustomSceneManager.load_scene("WolfBossRoom");
        //AudioManager.restore_sfx_smooth();
        unlink();
        yield break;
    }

    void unlink(){
        torches_on          = null;
        altar_numeral_on    = null;        
    }
}
