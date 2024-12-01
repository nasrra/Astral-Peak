using System;
using System.Collections;
using UnityEngine;

public class ShrineCutscene : Cutscene{
    public static event Action open_shrine_door;
    public override void start(){
        Player.instance.enter_cutscene_state();
        DialogueHandler.instance.play_dialogue(.05f);
        AudioManager.play_music(SoundID.DOMINE_THEME);
        DialogueHandler.instance.dialogue_ended += dialogue_ended;
        DialogueHandler.instance.new_line += handle_new_line;
    }

    void handle_new_line(int line){
        switch(line){
            case 27:
                CutsceneManager.invoke_event(open_shrine_door);
            break;
        }
    }

    public void dialogue_ended(){
        Player.instance.exit_cutscene_state();
        end();
    }

}
