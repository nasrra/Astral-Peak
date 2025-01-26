using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Sounds;
using UnityEngine.Audio;

public static class SoundLibrary{
    static Dictionary<SoundID, Sound> loaded_sounds = new Dictionary<SoundID, Sound>();

    public static void initialize(){
        SceneManager.sceneLoaded    += load_scene_sounds;
        SceneManager.sceneUnloaded  += unload_sounds;
    }

    public static void uninitialize(){
        loaded_sounds.Clear();
        SceneManager.sceneLoaded    -= load_scene_sounds;
        SceneManager.sceneUnloaded  -= unload_sounds;
    }

    public static Sound get_sound(SoundID id) => loaded_sounds.ContainsKey(id)? loaded_sounds[id] : throw new Exception(id + " has not been loaded.");
    static void load_scene_sounds(Scene scene, LoadSceneMode mode = LoadSceneMode.Single) => load_sounds(scene_sounds[scene.name]());
    static void load_sounds(List<Sound> sounds){
        // add the default "none sound"
        if(sounds == null)
            return;
        if(loaded_sounds.ContainsKey(SoundID.NONE) == false)
            loaded_sounds.Add(SoundID.NONE, new Sounds.None());
        foreach(Sound sound in sounds)
            loaded_sounds.Add(sound.id, sound);
    }
    static void unload_sounds(Scene scene) => loaded_sounds.Clear();

    public static AudioClip load(AudioMixerGroup group, string clip){
        if(group == AudioManager.music_mixer)
            return load_music(clip);
        else
            return load_sfx(clip);
    }

    public static AudioClip load_music(string audio_clip){
        AudioClip c = Resources.Load<AudioClip>("Audio/Music/"+audio_clip);
        return c != null? c : throw new System.Exception("Music Clip: ["+audio_clip+"] not found!");
    }

    public static AudioClip load_sfx(string audio_clip){
        AudioClip c = Resources.Load<AudioClip>("Audio/Sfx/"+audio_clip);
        return c != null? c : throw new System.Exception("Sfx Clip: ["+audio_clip+"] not found!");
    }

    static readonly Dictionary<string, Func<List<Sound>>> scene_sounds = new Dictionary<string, Func<List<Sound>>>(){
        {"temp",            () => {return null;}},
        {"DemoEnd",         () => {return null;}},
        {"Introduction",    () => {return new SceneSounds.Introduction().get_sounds();}},
        {"WolfBossRoom",    () => {return new SceneSounds.WolfBossRoom().get_sounds();}},
        {"SnowForest",      () => {return new SceneSounds.SnowForest().get_sounds();}},
        {"TutorialRoom",    () => {return new SceneSounds.TutorialRoom().get_sounds();}},
        {"Shrine",          () => {return new SceneSounds.Shrine().get_sounds();}},
        {"MainMenu",        () => {return new SceneSounds.MainMenu().get_sounds();}},
        {"Tower1",          () => {return new SceneSounds.Tower1().get_sounds();}},
        {"MageBossRoom",    () => {return new SceneSounds.MageBossRoom().get_sounds();}},
        {"SnowField",       () => {return new SceneSounds.SnowField().get_sounds();}}
    };
}



