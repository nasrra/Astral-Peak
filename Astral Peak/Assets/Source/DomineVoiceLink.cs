using System.Collections;
using System.Collections.Generic;
using DocumentFormat.OpenXml.Presentation;
using UnityEngine;

public class DomineVoiceLink : MonoBehaviour{
    [SerializeField] DialogueHandler dialogue;
    [SerializeField] AudioSource source;
    [SerializeField] List<AudioSpectrum> audio_spectrum = new List<AudioSpectrum>();
    void handle_new_line(int x){
        choose_voice();
        foreach(AudioSpectrum a in audio_spectrum)
            a.source = source;
    }
    //
    void OnEnable(){
        dialogue.new_line += handle_new_line;
    }

    void OnDisable(){
        dialogue.new_line -= handle_new_line;
    }

    void choose_voice(){
        int x = Random.Range(1, 5);
        SoundID sound;
        switch(x){
            case 1: sound = SoundID.DOMINE_VOICE_1; break;
            case 2: sound = SoundID.DOMINE_VOICE_2; break;
            case 3: sound = SoundID.DOMINE_VOICE_3; break;
            case 4: sound = SoundID.DOMINE_VOICE_4; break;
            default: sound = SoundID.BOW_SHOT; break;
        }

        AudioClipHandler.play(
            sound_id: sound, 
            randomise_pitch: true,
            spatial_blend: false, 
            audio_player: this, 
            loop: false, 
            out source
        );
    }
}
