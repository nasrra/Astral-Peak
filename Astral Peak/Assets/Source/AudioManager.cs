using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public static class AudioManager{
    public static void Play(string clip) => AudioClipHandler.instance.Play(SoundLibrary.sounds[clip]());
}

[System.Serializable]
public struct Sound{
    public Sound(AudioClip c, float v, float p){
        clip = c;
        volume = v;
        pitch = p;
    }
    public AudioClip clip;
    public float volume;
    public float pitch;
}

public static class SoundLibrary{
    public delegate Sound SoundCreation();
    public readonly static Dictionary<string, SoundCreation> sounds = new Dictionary<string, SoundCreation>(){
        { "boss1", () => new Sound(load_music("run - ABRN"), 1, 1) }
    };
    static AudioClip load_music(string audioclip){
        AudioClip c = Resources.Load<AudioClip>("Audio/Music/"+audioclip);
        return c != null? c : throw new System.Exception("Audio Clip: ["+audioclip+"] not found!");
    }
}
