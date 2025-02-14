using System.Collections.Generic;
using UnityEngine;
using Sounds;
using System;

public class DomineVoiceLink : MonoBehaviour{
    [SerializeField] AudioSource source;
    [SerializeField] List<AudioSpectrum> audio_spectrum = new List<AudioSpectrum>();
    [SerializeField] DomineVoiceLinesID id;
    VoiceLines voice_lines;

    void Awake() => voice_lines = create_voice_lines[id]();

    void Start(){
        DialogueHandler.instance.new_line += handle_new_line;
    }

    void OnDestroy(){
        DialogueHandler.instance.new_line -= handle_new_line;
    }

    void handle_new_line(int x){
        choose_voice(x);
        foreach(AudioSpectrum a in audio_spectrum)
            a.source = source;
    }

    Dictionary<DomineVoiceLinesID, Func<VoiceLines>> create_voice_lines = new Dictionary<DomineVoiceLinesID, Func<VoiceLines>>(){
        {DomineVoiceLinesID.SHRINE,             ()=>{return new DomineShrineVoiceLines();}},
        {DomineVoiceLinesID.ASTRAL_PLANE,       ()=>{return new DomineAstralPlaneVoiceLines();}},
    };

    void choose_voice(int x){
        SoundID sound = voice_lines.get_voice_lines().ContainsKey(x)? voice_lines.get_voice_lines()[x] : SoundID.NONE; 
        source = AudioClipHandler.play(
            sound_id: sound, 
            audio_player: gameObject, 
            AudioSourceSettings.NON_DIEGETIC);  
    }
}

public enum DomineVoiceLinesID : sbyte{
    SHRINE,
    ASTRAL_PLANE,
}

abstract class VoiceLines{
    protected Dictionary<int, SoundID> voice_lines;
    public Dictionary<int, SoundID> get_voice_lines() => voice_lines;
}

class DomineShrineVoiceLines : VoiceLines{
    public DomineShrineVoiceLines(){
        voice_lines = new Dictionary<int, SoundID>(){
        {0, SoundID.DOMINE_DEAD_WOMAN},
        {1, SoundID.DOMINE_SACRIFICE_RITUAL},
        {2, SoundID.DOMINE_AN_OFFERING},
        {3, SoundID.DOMINE_NO},
        {4, SoundID.DOMINE_CURSED_ONE},
        {5, SoundID.DOMINE_WHY_HERE},
        {6, SoundID.DOMINE_LOST_SOUL},
        {7, SoundID.DOMINE_BURNED_WOMAN},
        {8, SoundID.DOMINE_ASHES_TO_WIND},
        {9,SoundID.DOMINE_FOOL_OR_BRAVE},
        {10,SoundID.DOMINE_UNLESS},
        {11,SoundID.DOMINE_MOUNTAIN_SUMMIT}, // threshold of world.
        {12,SoundID.DOMINE_UNDER_ARCHES}, // last inhabitants.
        {13,SoundID.DOMINE_OLD_DOOR},
        {14,SoundID.DOMINE_WALK_AETHER}, // journey there/ wish granted.
        {15,SoundID.DOMINE_WARNING},
        {16,SoundID.DOMINE_ITS_EXPENSIVE},
        {17,SoundID.DOMINE_WAITING_MORTAL},
        };
    }
}

class DomineAstralPlaneVoiceLines : VoiceLines{
    public DomineAstralPlaneVoiceLines(){
        voice_lines = new Dictionary<int, SoundID>(){
        {1, SoundID.DOMINE_DEAD_WOMAN},
        {2, SoundID.DOMINE_SACRIFICE_RITUAL},
        {3, SoundID.DOMINE_AN_OFFERING},
        {4, SoundID.DOMINE_NO},
        {5, SoundID.DOMINE_CURSED_ONE},
        {6, SoundID.DOMINE_WHY_HERE},
        {7, SoundID.DOMINE_LOST_SOUL},
        {8, SoundID.DOMINE_BURNED_WOMAN},
        {9, SoundID.DOMINE_ASHES_TO_WIND},
        {10,SoundID.DOMINE_FOOL_OR_BRAVE},
        //11 ..
        {12,SoundID.DOMINE_UNLESS},
        //13 ...
        {14,SoundID.DOMINE_MOUNTAIN_SUMMIT}, // threshold of world.
        {15,SoundID.DOMINE_UNDER_ARCHES}, // last inhabitants.
        {16,SoundID.DOMINE_OLD_DOOR},
        {17,SoundID.DOMINE_WALK_AETHER}, // journey there/ wish granted.
        {18,SoundID.DOMINE_WARNING},
        {19,SoundID.DOMINE_ITS_EXPENSIVE},
        // 20 ...
        {21,SoundID.DOMINE_WAITING_MORTAL},            
        };
    }
}
