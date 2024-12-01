using System;
using System.Collections;
using DocumentFormat.OpenXml.Wordprocessing;
using UnityEngine;

public class ShrineCutscene : Cutscene{
    public static event Action open_shrine_door, torches_on, torches_off;
    public override void start(){
        Player.instance.enter_cutscene_state();
        DialogueHandler.instance.play_dialogue(3f);
        AudioManager.play_music(SoundID.DOMINE_THEME);
        DialogueHandler.instance.dialogue_ended += dialogue_ended;
        DialogueHandler.instance.new_line += handle_new_line;
        CutsceneManager.set_coroutine(camera_adjust());
    }

    IEnumerator camera_adjust(){
        CameraController.instance.zoom_in_state(4.4f, 1.6f);
        CameraController.instance.move_up_state(2.2f, 1.65f);
        yield return new WaitForSeconds(2f);
        CameraController.instance.zoom_in_state(4.45f, .2f);
        CameraController.instance.move_down_state(-2.5f, .2f);
        yield return new WaitForSeconds(48);
        CameraController.instance.reset_zoom_state(.2f);
        CameraController.instance.move_up_state(0, .2f);
        yield return new WaitForSeconds(20);
        yield break;
    }

    void handle_new_line(int line){
        switch(line){
            case 27: CutsceneManager.invoke_event(open_shrine_door); break;
            case 10: CutsceneManager.invoke_event(torches_on); break;
            case 11: CutsceneManager.invoke_event(torches_off); break;
            case 14: CutsceneManager.invoke_event(torches_on); break; 
        }
    }

    public void dialogue_ended(){
        Player.instance.exit_cutscene_state();
        AudioManager.stop_music();
        end();
    }

}
