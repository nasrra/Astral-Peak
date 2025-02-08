using System.Collections.Generic;
using UnityEngine;
using Sounds;

public class DomineVoiceLink : MonoBehaviour{
    [SerializeField] AudioSource source;
    [SerializeField] List<AudioSpectrum> audio_spectrum = new List<AudioSpectrum>();
    void handle_new_line(int x){
        choose_voice(x);
        foreach(AudioSpectrum a in audio_spectrum)
            a.source = source;
    }
    //
    void Start(){
        DialogueHandler.instance.new_line += handle_new_line;
    }

    void OnDestroy(){
        DialogueHandler.instance.new_line -= handle_new_line;
    }

    void choose_voice(int x){
        SoundID sound = voice_lines.ContainsKey(x)? voice_lines[x] : SoundID.NONE; 
        source = AudioClipHandler.play(
            sound_id: sound, 
            audio_player: gameObject, 
            AudioSourceSettings.NON_DIEGETIC);  
    }

    Dictionary<int, SoundID> voice_lines = new Dictionary<int, SoundID>(){
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

        //{6, SoundID.DOMINE_YOURE_MORTALS},
        //{13,SoundID.DOMINE_NO_HOPE},
        //{21,SoundID.DOMINE_REVIVE_WOMAN},
        //{23,SoundID.DOMINE_STRONG_WILL},
        //{24,SoundID.DOMINE_FIX_WOMAN},
        //{28,SoundID.DOMINE_ENTER_ROOM},
    };
}
