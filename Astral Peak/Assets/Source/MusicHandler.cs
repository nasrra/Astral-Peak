using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : AudioClipHandler{
    [SerializeField] AudioSource
        music_1, music_2,
        ambience_1, ambience_2;
    public static MusicManager instance;

    void Awake(){
        fade_factor = 0.5f;
        instance = this;  
    }
    
    public void play_music(Sound sound) => play(music_1, music_2, sound);
    public void play_ambience(Sound sound) => play(ambience_1, ambience_2, sound);

    public void play(AudioSource source_1, AudioSource source_2, Sound sound){
        if(source_1 != null){
            fade_out(source_1);
            fade_in(sound, out source_2);
        }
        else if(source_2 != null){
            fade_out(source_2);
            fade_in(sound, out source_1);         
        }
        else
            play(sound, out source_1);  
    }

    public void stop_music(){
        fade_out(music_1);
        fade_out(music_2);
    }
}
