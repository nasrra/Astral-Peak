using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using Entropek;
using FMODUnity;

public class ShrineOpeningCutscene : Cutscene{
    ShrineRoomHandler room = RoomHandler.instance as ShrineRoomHandler;
    #pragma warning disable CS0414
    public event Action 
        open_shrine_door, 
        torches_on, 
        torches_off, 
        enlargen_torches, 
        reset_torches;
    #pragma warning restore CS0414

    public override IEnumerator get_coroutine() => start();
    IEnumerator start(){
        CameraController.instance.regulate = false;
        AudioManager.load_bank("cutscene_shrine");
        AudioManager.play_music_one_shot("music_domine");
        AudioManager.stop_ambience();
        DialogueHandler.instance.dialogue_ended += dialogue_ended;
        DialogueHandler.instance.new_line += handle_new_line;
        DialogueHandler.instance.set_dialogue(ExcelReader.read_file("Dialogue", "DomineShrine"));
        torches_on?.Invoke();
        CameraController.instance.lerp_zoom(4.45f, 7f);
        CameraController.instance.lerp_offset(null, 2.2f, 7f);
        yield return new WaitForSeconds(7f);
        DialogueHandler.instance.play_dialogue(2.7f);
        CameraController.instance.lerp_offset(null,-2.5f, 17f);
        yield return new WaitForSeconds(29);
        CameraController.instance.reset_zoom(20);
        CameraController.instance.lerp_offset(null, 0, 20f);     
        yield break;//
    }

    IEnumerator middle(){
        CameraController.instance.lerp_zoom(2f, 7f);
        CameraController.instance.lerp_offset(-3, null, 7f);
        yield return new WaitForSeconds(7);
        CameraController.instance.lerp_offset(3.4f,null, 25f);
        yield return new WaitForSeconds(25);
        CameraController.instance.reset_zoom(10f);
        CameraController.instance.reset_offset(10f);
        yield break;
    }


    IEnumerator ending(){
        torches_off?.Invoke();     
        stop_skip();
        yield return new WaitForSeconds(6);  
        CameraController.instance.regulate = true;
        AudioManager.stop_music();
        AudioManager.play_ambience("ambience_shrine");
        UnityHook.instance.StartCoroutine(Util.timer(4,time_out:()=>AudioManager.unload_bank("cutscene_shrine")));
        unlink();
        end();
        yield break;
    }
    public void dialogue_ended() => CutsceneManager.set_coroutine(ending());
    void handle_new_line(int line){
        switch(line){
            case 20: 
                open_shrine_door?.Invoke(); 
                break;
            case 7: 
                enlargen_torches?.Invoke();
                break;
            case 8: 
                reset_torches?.Invoke(); 
                torches_off?.Invoke(); 
                break;
            case 10: 
                reset_torches?.Invoke(); 
                torches_on?.Invoke(); 
                break;
            case 12: 
                CutsceneManager.set_coroutine(middle()); 
                break;
            case 13: 
                room.gateway_constellation.fade_in();
                break;
            case 18:
                room.gateway_constellation.fade_out();
                break;

        }
    }

    void unlink(){
        open_shrine_door            = null; 
        torches_on                  = null;
        torches_off                 = null;
        enlargen_torches            = null;
        reset_torches               = null;        
    }

}

public abstract class ShrineAltarCutscene : Cutscene{
    public event Action
        torches_on, torches_off;
    public event Action<List<int>>
        turn_on_numeral, set_numerals;
    public abstract List<int> get_set_numerals();
    public abstract List<int> get_turn_on_numeral();
    public abstract string get_previous_scene();

    public override IEnumerator get_coroutine(){
        CustomSceneManager.load_scene_with_transitions("Shrine");
        CustomSceneManager.loaded_scene += start_altar_cutscene;
        yield break;
    }

    void start_altar_cutscene(){
        CustomSceneManager.loaded_scene -= start_altar_cutscene;
        CutsceneManager.set_coroutine(altar_cutscene());
    }

    public IEnumerator altar_cutscene(){
        AudioManager.load_bank("cutscene_altar");
        AudioManager.stop_ambience();
        AudioManager.stop_music();
        AudioManager.play_music("music_altar");
        Player.instance.gameObject.SetActive(false);
        CameraEffects.instance.flashback_state();
        set_numerals?.Invoke(get_set_numerals());
        
        yield return new WaitForSeconds(4f);
        torches_on?.Invoke();
        
        yield return new WaitForSeconds(8);
        turn_on_numeral?.Invoke(get_turn_on_numeral());
        
        yield return new WaitForSeconds(6);
        torches_off?.Invoke();
        yield return new WaitForSeconds(6);
        AudioManager.stop_music();
        UnityHook.instance.StartCoroutine(Util.timer(4,time_out:()=>AudioManager.unload_bank("cutscene_altar")));
        CustomSceneManager.load_scene_with_transitions(get_previous_scene());
        CustomSceneManager.temp_scene += stop_skip;
        CustomSceneManager.temp_scene += end;
        CustomSceneManager.temp_scene += unlink;
        yield break;
    }
    
    void unlink(){
        CustomSceneManager.temp_scene -= stop_skip;
        CustomSceneManager.temp_scene -= end;
        CustomSceneManager.temp_scene -= unlink;
        torches_off         = null;
        torches_on          = null;
        turn_on_numeral     = null;        
    }    
}

public class ShrineAltarOneCutscene : ShrineAltarCutscene{
    public override IEnumerator get_coroutine(){        
        yield return base.get_coroutine();
    }

    public override string get_previous_scene()=>"CavalryBossRoom";

    public override List<int> get_set_numerals() => null;
    public override List<int> get_turn_on_numeral()=>new(){
        0,
    };
}

public class ShrineAltarTwoCutscene : ShrineAltarCutscene{
    public override string get_previous_scene()=>"MageBossRoom";

    public override List<int> get_set_numerals()=>new(){
        0,
    };

    public override List<int> get_turn_on_numeral()=>new(){
        1,
    };
}

public class ShrineAltarThreeCutscene : ShrineAltarCutscene{
    public override string get_previous_scene()=>"GiantBossRoom";
    // public override string get_previous_scene()=>"DemoEnd";

    public override List<int> get_set_numerals()=>new(){
        0,1
    };

    public override List<int> get_turn_on_numeral()=>new(){
        2,
    };
}