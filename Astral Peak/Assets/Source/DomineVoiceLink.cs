using System.Collections;
using System.Collections.Generic;
using DocumentFormat.OpenXml.Presentation;
using UnityEngine;
using Sounds;

public class DomineVoiceLink : MonoBehaviour{
    [SerializeField] AudioSource source;
    [SerializeField] List<AudioSpectrum> audio_spectrum = new List<AudioSpectrum>();
    void handle_new_line(int x){
        choose_voice();
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

    void choose_voice(){
        int x = Random.Range(1, 5);
        SoundID sound;
        switch(x){
            case 1: sound = SoundID.DANIEL; break;
            case 2: sound = SoundID.DANIEL; break;
            case 3: sound = SoundID.DANIEL; break;
            case 4: sound = SoundID.DANIEL; break;
            default: sound = SoundID.BOW_SHOT; break;
        }

        source = AudioClipHandler.play(
            sound_id: sound, 
            audio_player: this, 
            AudioSourceSettings.NON_DIEGETIC);  
    }
}
