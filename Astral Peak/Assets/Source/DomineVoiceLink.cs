using System.Collections.Generic;
using UnityEngine;
using System;

public class DomineVoiceLink : MonoBehaviour{
    [SerializeField] List<AudioSpectrum> audio_spectrum = new List<AudioSpectrum>();
    [SerializeField] DomineVoiceLinesID id;
    [SerializeField] AudioPlayer audio_player;
    VoiceLines voice_lines;

    void Awake(){
        voice_lines = create_voice_lines[id]();
        audio_player.initialize(create_audio_player[id]());
    }   

    void Start(){
        DialogueHandler.instance.new_line += handle_new_line;
    }

    void OnDestroy(){
        DialogueHandler.instance.new_line -= handle_new_line;
    }

    void handle_new_line(int x){
        choose_voice(x);
        //foreach(AudioSpectrum a in audio_spectrum)
        //    a.source = source;
    }

    Dictionary<DomineVoiceLinesID, Func<VoiceLines>> create_voice_lines = new Dictionary<DomineVoiceLinesID, Func<VoiceLines>>(){
        {DomineVoiceLinesID.SHRINE,             ()=>{return new DomineShrineVoiceLines();}},
        {DomineVoiceLinesID.ASTRAL_PLANE,       ()=>{return new DomineAstralPlaneVoiceLines();}},
    };
    Dictionary<DomineVoiceLinesID, Func<AudioPlayerEventDataPackage>> create_audio_player = new Dictionary<DomineVoiceLinesID, Func<AudioPlayerEventDataPackage>>(){
        {DomineVoiceLinesID.SHRINE,             ()=>{return new DomineShrineAudioPlayerEventDataPackage();}},
        {DomineVoiceLinesID.ASTRAL_PLANE,       ()=>{return new DomineAstralPlaneAudioPlayerEventDataPackage();}},        
    };

    void choose_voice(int x){
        if(voice_lines.get_voice_lines().ContainsKey(x)){
            audio_player.play_non_diegetic_one_shot(voice_lines.get_voice_lines()[x]);
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
        {0, "domine_dead_woman"},
        {1, "domine_sacrifice_ritual"},
        {2, "domine_an_offering"},
        {3, "domine_no"},
        {4, "domine_cursed_one"},
        {5, "domine_why_here"},
        {6, "domine_lost_soul"},
        {7, "domine_burned_woman"},
        {8, "domine_ashes_to_wind"},
        {9, "domine_fool_or_brave"},
        {10,"domine_unless"},
        {11,"domine_mountain_summit"}, // threshold of world.
        {12,"domine_under_arches"}, // last inhabitants.
        {13,"domine_old_door"},
        {14,"domine_walk_aether"}, // journey there/ wish granted.
        {15,"domine_warning"},
        {16,"domine_its_expensive"},
        {17,"domine_waiting_mortal"},
        };
    }
}

struct DomineAstralPlaneAudioPlayerEventDataPackage : AudioPlayerEventDataPackage{
    public List<AudioPlayerEventData> get_event_instances(){
        return new List<AudioPlayerEventData>();
    }

    public List<AudioPlayerEventData> get_event_references(){
        return new List<AudioPlayerEventData>(){
            new(FMODEventFolders.VOICE_DOMINE,"domine_dead_woman"),
            new(FMODEventFolders.VOICE_DOMINE,"domine_sacrifice_ritual"),
            new(FMODEventFolders.VOICE_DOMINE,"domine_an_offering"),
            new(FMODEventFolders.VOICE_DOMINE,"domine_no"),
            new(FMODEventFolders.VOICE_DOMINE,"domine_cursed_one"),
            new(FMODEventFolders.VOICE_DOMINE,"domine_why_here"),
            new(FMODEventFolders.VOICE_DOMINE,"domine_lost_soul"),
            new(FMODEventFolders.VOICE_DOMINE,"domine_burned_woman"),
            new(FMODEventFolders.VOICE_DOMINE,"domine_ashes_to_wind"),
            new(FMODEventFolders.VOICE_DOMINE,"domine_fool_or_brave"),
            new(FMODEventFolders.VOICE_DOMINE,"domine_unless"),
            new(FMODEventFolders.VOICE_DOMINE,"domine_mountain_summit"), // threshold of world.
            new(FMODEventFolders.VOICE_DOMINE,"domine_under_arches"), // last inhabitants.
            new(FMODEventFolders.VOICE_DOMINE,"domine_old_door"),
            new(FMODEventFolders.VOICE_DOMINE,"domine_walk_aether"), // journey there/ wish granted.
            new(FMODEventFolders.VOICE_DOMINE,"domine_warning"),
            new(FMODEventFolders.VOICE_DOMINE,"domine_its_expensive"),
            new(FMODEventFolders.VOICE_DOMINE,"domine_waiting_mortal"),
        };
    }
}

class DomineShrineVoiceLines : VoiceLines{
    public DomineShrineVoiceLines(){
        voice_lines = new Dictionary<int, string>(){
        
        {1, "domine_dead_woman"},
        {2, "domine_sacrifice_ritual"},
        {3, "domine_an_offering"},
        {4, "domine_no"},
        {5, "domine_cursed_one"},
        {6, "domine_why_here"},
        {7, "domine_lost_soul"},
        {8, "domine_burned_woman"},
        {9, "domine_ashes_to_wind"},
        {10, "domine_fool_or_brave"},
        //11...
        {12,"domine_unless"},
        //13...
        {14,"domine_mountain_summit"}, // threshold of world.
        {15,"domine_under_arches"}, // last inhabitants.
        {16,"domine_old_door"},
        {17,"domine_walk_aether"}, // journey there/ wish granted.
        {18,"domine_warning"},
        {19,"domine_its_expensive"},
        //20...
        {21,"domine_waiting_mortal"},
        };       
    }
}

struct DomineShrineAudioPlayerEventDataPackage : AudioPlayerEventDataPackage{
    public List<AudioPlayerEventData> get_event_instances(){
        return new List<AudioPlayerEventData>();
    }

    public List<AudioPlayerEventData> get_event_references(){
        return new List<AudioPlayerEventData>(){
            new(FMODEventFolders.VOICE_DOMINE,"domine_dead_woman"),
            new(FMODEventFolders.VOICE_DOMINE,"domine_sacrifice_ritual"),
            new(FMODEventFolders.VOICE_DOMINE,"domine_an_offering"),
            new(FMODEventFolders.VOICE_DOMINE,"domine_no"),
            new(FMODEventFolders.VOICE_DOMINE,"domine_cursed_one"),
            new(FMODEventFolders.VOICE_DOMINE,"domine_why_here"),
            new(FMODEventFolders.VOICE_DOMINE,"domine_lost_soul"),
            new(FMODEventFolders.VOICE_DOMINE,"domine_burned_woman"),
            new(FMODEventFolders.VOICE_DOMINE,"domine_ashes_to_wind"),
            new(FMODEventFolders.VOICE_DOMINE,"domine_fool_or_brave"),
            new(FMODEventFolders.VOICE_DOMINE,"domine_unless"),
            new(FMODEventFolders.VOICE_DOMINE,"domine_mountain_summit"), // threshold of world.
            new(FMODEventFolders.VOICE_DOMINE,"domine_under_arches"), // last inhabitants.
            new(FMODEventFolders.VOICE_DOMINE,"domine_old_door"),
            new(FMODEventFolders.VOICE_DOMINE,"domine_walk_aether"), // journey there/ wish granted.
            new(FMODEventFolders.VOICE_DOMINE,"domine_warning"),
            new(FMODEventFolders.VOICE_DOMINE,"domine_its_expensive"),
            new(FMODEventFolders.VOICE_DOMINE,"domine_waiting_mortal"),
        };
    }
}
}