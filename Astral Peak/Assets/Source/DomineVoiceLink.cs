using System.Collections.Generic;
using UnityEngine;
using System;
using FMODUnity;


public class DomineVoiceLink : MonoBehaviour{
    [SerializeField] AudioSpectrum audio_spectrum;
    [SerializeField] DomineVoiceLinesID id;
    [SerializeField] AudioPlayer audio_player;
    VoiceLines voice_lines;

    void Awake(){
        voice_lines = create_voice_lines[id]();
    }   

    void Start(){
        DialogueHandler.instance.dialogue_started   += dialogue_started;
        DialogueHandler.instance.new_line           += handle_new_line;
        DialogueHandler.instance.dialogue_ended     += dialogue_ended;
    }

    void OnDestroy(){
        DialogueHandler.instance.dialogue_started   -= dialogue_started;
        DialogueHandler.instance.new_line           -= handle_new_line;
        DialogueHandler.instance.dialogue_ended     -= dialogue_ended;
    }

    void handle_new_line(int x){
        choose_voice(x);
    }

    Dictionary<DomineVoiceLinesID, Func<VoiceLines>> create_voice_lines = new Dictionary<DomineVoiceLinesID, Func<VoiceLines>>(){
        {DomineVoiceLinesID.SHRINE,             ()=>{return new DomineShrineVoiceLines();}},
        {DomineVoiceLinesID.ASTRAL_PLANE,       ()=>{return new DomineAstralPlaneVoiceLines();}},
    };

    private void dialogue_started(){
        audio_spectrum.initialize(RuntimeManager.GetBus("bus:/voice"));
    }

    private void dialogue_ended(){
        audio_spectrum.uninitialize();
    }

    void choose_voice(int x){
        if(voice_lines.get_voice_lines().ContainsKey(x)){
            audio_player.play_non_diegetic_one_shot_instance(voice_lines.get_voice_lines()[x]);
    }
}

public enum DomineVoiceLinesID : sbyte{
    SHRINE,
    ASTRAL_PLANE,
}

abstract class VoiceLines{
    protected Dictionary<int, string> voice_lines;
    public Dictionary<int, string> get_voice_lines() => voice_lines;
}

class DomineAstralPlaneVoiceLines : VoiceLines{
    public DomineAstralPlaneVoiceLines(){
        voice_lines = new Dictionary<int, string>(){
        {0, "domine_arrived_mortal"},
        {1, "domine_astral_plane"}, // welcome
        {2, "domine_wish_permitted"}, // wish granted.
        {3, "domine_unless"}, // however.
        // 4...
        {5, "domine_its_expensive"}, // ressurcetion costly
        {6, "domine_solo_enter"}, // one enter
        {7, "domine_solo_exit"}, // one leave.
        {8, "domine_no_reunion"}, // no reunited.
        // 9...
        {10, "domine_you_cannot_leave"}, // cannot leave.
        {11, "domine_find_peace"}, // may you find peace.
        {12, "domine_farewell_mortal"}, // farewell mortal.
        };
    }
}

class DomineShrineVoiceLines : VoiceLines{
    public DomineShrineVoiceLines(){
        voice_lines = new Dictionary<int, string>(){
        
        // {0, "domine_dead_woman"},
        {1, "domine_dead_woman"},
        {2, "domine_sacrifice_ritual"},
        {3, "domine_an_offering"},
        {4, "domine_no"},
        {5, "domine_cursed_one"},
        {6, "domine_why_here"},
        {7, "domine_woman_lost_soul"},
        {8, "domine_remains_torched"},
        {9, "domine_ashes_forgotten"}, 
        {10, "domine_fool_or_brave"},
        //11...
        {12,"domine_unless"},
        //13...
        {14,"domine_sealed_away_door"}, // sealed away door.
        {15,"domine_guarded_tomb"}, // guarded tomb.
        {16,"domine_woman_resurrection"}, // woman resurrection.
        {17,"domine_warning"},
        //18...
        {19,"domine_its_expensive"},
        //20...
        {21,"domine_waiting_mortal"},
        };       
    }
}
}