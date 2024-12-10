using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Sounds;

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
        if(sounds == null)
            return;
        foreach(Sound sound in sounds)
            loaded_sounds.Add(sound.id(), sound);
        // add the default "none sound"
        loaded_sounds.Add(SoundID.NONE, new Sounds.None());
    }
    static void unload_sounds(Scene scene) => loaded_sounds.Clear();

    public static AudioClip load_music(string audio_clip){
        AudioClip c = Resources.Load<AudioClip>("Audio/Music/"+audio_clip);
        return c != null? c : throw new System.Exception("Music Clip: ["+audio_clip+"] not found!");
    }

    public static AudioClip load_sfx(string audio_clip){
        AudioClip c = Resources.Load<AudioClip>("Audio/Sfx/"+audio_clip);
        return c != null? c : throw new System.Exception("Sfx Clip: ["+audio_clip+"] not found!");
    }

    static readonly Dictionary<string, Func<List<Sound>>> scene_sounds = new Dictionary<string, Func<List<Sound>>>(){
        {"WolfBossRoom", () => {return new WolfBossRoomSceneSounds().get_sounds();}},
        {"SnowForest", () => {return new SnowForestSceneSounds().get_sounds();}},
        {"TutorialRoom", () => {return new TutorialRoomSceneSounds().get_sounds();}},
        {"Shrine",() => {return new ShrineSceneSounds().get_sounds();}},
        {"temp",() => {return null;}},
        {"MainMenu",() => {return new MainMenuSceneSounds().get_sounds();}},
    };
}



