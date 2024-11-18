using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

[System.Serializable]
public struct Sound{
    public Sound(AudioClip c, AudioMixerGroup g, float v, float p = 1){
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

public enum SoundID{
    // MUSIC
    WOLF_BOSS_MUSIC,

    // SFX
    BOW_SHOT,
    COIN_TOSS,
    DOG_BARK_1,
    DOG_BARK_2,
    DOG_BARK_3,
    LEATHER_CONTORT_1,
    MAGIC_1,
    MELEE_SWING_1,
    MELEE_SWING_2,
    MELEE_SWING_3,
    RIDER_YELL,
    SNOW_FOOTSTEP_1,
    SNOW_FOOTSTEP_2,
    SNOW_FOOTSTEP_3,
    SNOW_FOOTSTEP_4,
    SNOW_IMPACT_HEAVY,
    SOFT_WIND,
    WHOOSH_1,
    WHISTLE_LONG,
    WOLF_HOWL
}

// optimisation: 
// - have a dictionary of loaded sounds to hook into for objects. Objscts call for the ir creation on awake(), system checks if sound is loaded (load if not, dont if already).
// - unload the sounds when a new scene is loaded, so this by hooking into unity hook.

public static class SoundLibrary{
    static Dictionary<SoundID, Sound> loaded_sounds = new Dictionary<SoundID, Sound>();

    public static void initialize(){
        SceneManager.sceneLoaded    += load_scene_sounds;
        SceneManager.sceneUnloaded  += unload_sounds;
    }

    public static Sound get_sound(SoundID id) => loaded_sounds.ContainsKey(id)? loaded_sounds[id] : throw new Exception(id + " has not been loaded.");
    static void load_scene_sounds(Scene scene, LoadSceneMode mode = LoadSceneMode.Single){
        Debug.Log("Load ["+scene.name+"] sounds.");
        load_sounds(SceneSounds.create[scene.name]());
    } 
    static void load_sounds(List<SoundID> sounds){
        foreach(SoundID name in sounds)
            loaded_sounds.Add(name, sound_creation[name]());
    }
    static void unload_sounds(Scene scene) => loaded_sounds.Clear();

    static AudioClip load_music(string audio_clip){
        AudioClip c = Resources.Load<AudioClip>("Audio/Music/"+audio_clip);
        return c != null? c : throw new System.Exception("Music Clip: ["+audio_clip+"] not found!");
    }

    static AudioClip load_sfx(string audio_clip){
        AudioClip c = Resources.Load<AudioClip>("Audio/Sfx/"+audio_clip);
        return c != null? c : throw new System.Exception("Sfx Clip: ["+audio_clip+"] not found!");
    }

    public readonly static Dictionary<SoundID, Func<Sound>> sound_creation = new Dictionary<SoundID, Func<Sound>>(){
        
        //MUSIC
        {SoundID.WOLF_BOSS_MUSIC,     ()=> new Sound(load_music("run - ABRN"), AudioManager.music_mixer, 1, 1)},

        //SFX
        {SoundID.SNOW_FOOTSTEP_1,     () => new Sound(load_sfx("snow_footstep_1"),    AudioManager.sfx_mixer,.5f)},
        {SoundID.SNOW_FOOTSTEP_2,     () => new Sound(load_sfx("snow_footstep_2"),    AudioManager.sfx_mixer,.5f)},
        {SoundID.SNOW_FOOTSTEP_3,     () => new Sound(load_sfx("snow_footstep_3"),    AudioManager.sfx_mixer,.5f)},
        {SoundID.SNOW_FOOTSTEP_4,     () => new Sound(load_sfx("snow_footstep_4"),    AudioManager.sfx_mixer,.5f)},
        {SoundID.SOFT_WIND,           () => new Sound(load_sfx("soft_wind"),          AudioManager.sfx_mixer, 1f)},
        {SoundID.WOLF_HOWL,           () => new Sound(load_sfx("wolf_howl"),          AudioManager.sfx_mixer, 1f)},
        {SoundID.MELEE_SWING_1,       () => new Sound(load_sfx("melee_swing_1"),      AudioManager.sfx_mixer, 1f)},
        {SoundID.MELEE_SWING_2,       () => new Sound(load_sfx("melee_swing_2"),      AudioManager.sfx_mixer, 1f)},
        {SoundID.MELEE_SWING_3,       () => new Sound(load_sfx("melee_swing_3"),      AudioManager.sfx_mixer, 1f)},
        {SoundID.MAGIC_1,             () => new Sound(load_sfx("magic_1"),            AudioManager.sfx_mixer, 1f)},
        {SoundID.SNOW_IMPACT_HEAVY,   () => new Sound(load_sfx("snow_impact_heavy"),  AudioManager.sfx_mixer,.8f)},
        {SoundID.WHISTLE_LONG,        () => new Sound(load_sfx("whistle_long"),       AudioManager.sfx_mixer, 1f)},
        {SoundID.WHOOSH_1,            () => new Sound(load_sfx("whoosh_1"),           AudioManager.sfx_mixer, 1f)},
        {SoundID.DOG_BARK_1,          () => new Sound(load_sfx("dog_bark_1"),         AudioManager.sfx_mixer, 1f)},
        {SoundID.DOG_BARK_2,          () => new Sound(load_sfx("dog_bark_2"),         AudioManager.sfx_mixer, 1f)},
        {SoundID.DOG_BARK_3,          () => new Sound(load_sfx("dog_bark_3"),         AudioManager.sfx_mixer, 1f)},
        {SoundID.LEATHER_CONTORT_1,   () => new Sound(load_sfx("leather_contort_1"),  AudioManager.sfx_mixer, 1f)},
        {SoundID.BOW_SHOT,            () => new Sound(load_sfx("bow_shot"),           AudioManager.sfx_mixer, 1f)},
        {SoundID.COIN_TOSS,           () => new Sound(load_sfx("coin_toss"),          AudioManager.sfx_mixer, 1f)},
        {SoundID.RIDER_YELL,          () => new Sound(load_sfx("rider_yell"),         AudioManager.sfx_mixer,.6f)},
    };
}



