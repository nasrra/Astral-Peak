using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

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
    
    public readonly static Dictionary<string, SoundCreation> music = new Dictionary<string, SoundCreation>(){
        {"boss1", () => new Sound(load_music("run - ABRN"), AudioManager.music_mixer, 1, 1)}
    };

    public readonly static Dictionary<string, SoundCreation> sfx = new Dictionary<string, SoundCreation>(){
        {"snow_footstep_1",     () => new Sound(load_sfx("snow_footstep_1"), AudioManager.sfx_mixer, 0.5f, 1)},
        {"snow_footstep_2",     () => new Sound(load_sfx("snow_footstep_2"), AudioManager.sfx_mixer, 0.5f, 1)},
        {"snow_footstep_3",     () => new Sound(load_sfx("snow_footstep_3"), AudioManager.sfx_mixer, 0.5f, 1)},
        {"snow_footstep_4",     () => new Sound(load_sfx("snow_footstep_4"), AudioManager.sfx_mixer, 0.5f, 1)},
        {"wind",                () => new Sound(load_sfx("soft_wind"), AudioManager.sfx_mixer, 1f, 1)},
        {"wolf_howl",           () => new Sound(load_sfx("wolf_howl"), AudioManager.sfx_mixer, 1,1)},
        {"melee_swing_1",       () => new Sound(load_sfx("melee_swing_1"), AudioManager.sfx_mixer, 1f,1)},
        {"melee_swing_2",       () => new Sound(load_sfx("melee_swing_2"), AudioManager.sfx_mixer, 1f,1)},
        {"melee_swing_3",       () => new Sound(load_sfx("melee_swing_3"), AudioManager.sfx_mixer, 1f,1)},
        {"magic_1",             () => new Sound(load_sfx("magic_1"), AudioManager.sfx_mixer, 1f,1f)},
        {"snow_impact_heavy",   () => new Sound(load_sfx("snow_impact_heavy"), AudioManager.sfx_mixer,.8f,.8f)},
        {"whoosh_1",            () => new Sound(load_sfx("whoosh_1"), AudioManager.sfx_mixer,1f,1f)},
        {"dog_bark_1",          () => new Sound(load_sfx("dog_bark_1"), AudioManager.sfx_mixer,1f,1f)},
        {"dog_bark_2",          () => new Sound(load_sfx("dog_bark_2"), AudioManager.sfx_mixer,1f,1f)},
        {"dog_bark_3",          () => new Sound(load_sfx("dog_bark_3"), AudioManager.sfx_mixer,1f,1f)},
        {"leather_contort_1",   () => new Sound(load_sfx("leather_contort_1"), AudioManager.sfx_mixer, 1f, 1f)},
        {"bow_shot",            () => new Sound(load_sfx("bow_shot"), AudioManager.sfx_mixer, 1f, 1f)},
        {"coin_toss",           () => new Sound(load_sfx("coin_toss"), AudioManager.sfx_mixer, 1f, 1f)}
    };

    static AudioClip load_music(string audio_clip){
        AudioClip c = Resources.Load<AudioClip>("Audio/Music/"+audio_clip);
        return c != null? c : throw new System.Exception("Music Clip: ["+audio_clip+"] not found!");
    }

    static AudioClip load_sfx(string audio_clip){
        AudioClip c = Resources.Load<AudioClip>("Audio/Sfx/"+audio_clip);
        return c != null? c : throw new System.Exception("Sfx Clip: ["+audio_clip+"] not found!");
    }
}
