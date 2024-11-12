using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public static class AudioManager{
    public static AudioMixer mixer = Resources.Load<AudioMixer>("Audio/Mixer");
    public static void Play(string clip) => AudioClipHandler.instance.Play(SoundLibrary.sounds[clip]());
    public static void music_volume(float volume) => mixer.SetFloat("MusicVolume",value_to_logarithmic(volume));

    // calc for mixer because volume levels are set by logarithmic values.
    static float value_to_logarithmic(float value) => Mathf.Log10(value) * 20; 
}

[System.Serializable]
public struct Sound{
    public Sound(AudioClip c, AudioMixerGroup g, float v, float p){
        clip = c;
        group = g;
        volume = v;
        pitch = p;
    }
    public AudioClip clip;
    public AudioMixerGroup group;
    public float volume;
    public float pitch;
}

public static class SoundLibrary{
    public delegate Sound SoundCreation();
    
    public readonly static Dictionary<string, SoundCreation> sounds = new Dictionary<string, SoundCreation>(){
        { "boss1", 
            () => new Sound(
                load_music("run - ABRN"), 
                AudioManager.mixer.FindMatchingGroups("Music")[0], 
                1, 
                1) 
        }
    };

    static AudioClip load_music(string audioclip){
        AudioClip c = Resources.Load<AudioClip>("Audio/Music/"+audioclip);
        return c != null? c : throw new System.Exception("Audio Clip: ["+audioclip+"] not found!");
    }
}
