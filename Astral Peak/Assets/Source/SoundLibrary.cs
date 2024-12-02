using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

[System.Serializable]
public struct Sound{
    public Sound(AudioClip _clip, AudioMixerGroup _group, float _volume = 1, float _max_pitch = 1, float _min_pitch = 1){
        clip = _clip;
        group = _group;
        volume = _volume;
        max_pitch = _max_pitch;
        min_pitch = _min_pitch;
    }
    public AudioClip clip;
    public AudioMixerGroup group;
    public float volume;
    public float max_pitch;
    public float min_pitch;
    public float randomise_pitch() => UnityEngine.Random.Range(min_pitch, max_pitch);
}

public enum SoundID{
    // MUSIC
    WOLF_BOSS_MUSIC_1,
    WOLF_BOSS_MUSIC_2,
    DOMINE_THEME,

    // SFX
    BOW_SHOT,
    COIN_TOSS,
    DEEP_BOOM,
    DOG_BARK_1,
    LEATHER_CONTORT_1,
    MAGIC_1,
    MAGIC_EXPLOSION,
    MELEE_HIT,
    MELEE_SWING_1,
    MELEE_SWING_2,
    MELEE_SWING_3,
    RIDER_YELL,
    SNOW_FOOTSTEP_1,
    SNOW_FOOTSTEP_2,
    SNOW_FOOTSTEP_3,
    SNOW_FOOTSTEP_4,
    SNOW_JUMP,
    SNOW_IMPACT_HEAVY,
    SNOW_IMPACT_LIGHT,
    SOFT_WIND,
    STEAM,
    WHOOSH_1,
    WHISTLE_LONG,
    WOLF_HOWL,
    WOODEN_PING,
    STONE_FOOTSTEP_1,
    STONE_FOOTSTEP_2,
    STONE_FOOTSTEP_3,
    STONE_FOOTSTEP_4,
    STONE_IMPACT_LIGHT,
    DOMINE_VOICE_1,
    DOMINE_VOICE_2,
    DOMINE_VOICE_3,
    DOMINE_VOICE_4,
    SMALL_FIRE,
    NONE,
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

    public static void uninitialize(){
        loaded_sounds.Clear();
        SceneManager.sceneLoaded    -= load_scene_sounds;
        SceneManager.sceneUnloaded  -= unload_sounds;
    }

    public static Sound get_sound(SoundID id) => loaded_sounds.ContainsKey(id)? loaded_sounds[id] : throw new Exception(id + " has not been loaded.");
    static void load_scene_sounds(Scene scene, LoadSceneMode mode = LoadSceneMode.Single) => load_sounds(SceneSounds.create[scene.name]());
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
        {SoundID.WOLF_BOSS_MUSIC_1,     ()=> new Sound(load_music("ABRN_run_part_1"), AudioManager.music_mixer, 0.8f, 1)},
        {SoundID.WOLF_BOSS_MUSIC_2,     ()=> new Sound(load_music("ABRN_run_part_2"), AudioManager.music_mixer, 0.8f, 1)},
        {SoundID.DOMINE_THEME,          ()=> new Sound(load_music("domine theme"), AudioManager.music_mixer, 0.8f, 1)},

        //SFX
        {SoundID.SNOW_FOOTSTEP_1,     () => new Sound(load_sfx("snow_footstep_1"),    AudioManager.sfx_mixer, 1f, 1.15f, 0.85f)},
        {SoundID.SNOW_FOOTSTEP_2,     () => new Sound(load_sfx("snow_footstep_2"),    AudioManager.sfx_mixer, 1f, 1.15f, 0.85f)},
        {SoundID.SNOW_FOOTSTEP_3,     () => new Sound(load_sfx("snow_footstep_3"),    AudioManager.sfx_mixer, 1f, 1.15f, 0.85f)},
        {SoundID.SNOW_FOOTSTEP_4,     () => new Sound(load_sfx("snow_footstep_4"),    AudioManager.sfx_mixer, 1f, 1.15f, 0.85f)},
        {SoundID.STONE_FOOTSTEP_1,    () => new Sound(load_sfx("stone_footstep_1"),   AudioManager.sfx_mixer, 1f, 1.15f, 0.85f)},
        {SoundID.STONE_FOOTSTEP_2,    () => new Sound(load_sfx("stone_footstep_2"),   AudioManager.sfx_mixer, 1f, 1.15f, 0.85f)},
        {SoundID.STONE_FOOTSTEP_3,    () => new Sound(load_sfx("stone_footstep_3"),   AudioManager.sfx_mixer, 1f, 1.15f, 0.85f)},
        {SoundID.STONE_FOOTSTEP_4,    () => new Sound(load_sfx("stone_footstep_4"),   AudioManager.sfx_mixer, 1f, 1.15f, 0.85f)},
        {SoundID.STONE_IMPACT_LIGHT,  () => new Sound(load_sfx("stone_impact_light"), AudioManager.sfx_mixer, 1f, 1.15f, 0.85f)},
        {SoundID.SNOW_JUMP,           () => new Sound(load_sfx("snow_jump"),          AudioManager.sfx_mixer, 1f, 1.15f, 0.85f)},
        {SoundID.SOFT_WIND,           () => new Sound(load_sfx("soft_wind"),          AudioManager.sfx_mixer, 0.8f, 1.15f, 0.85f)},
        {SoundID.WOLF_HOWL,           () => new Sound(load_sfx("wolf_howl"),          AudioManager.sfx_mixer, 1f)},
        {SoundID.MELEE_HIT,           () => new Sound(load_sfx("melee_hit"),          AudioManager.sfx_mixer, 1f, 1.15f, 0.85f)},
        {SoundID.MELEE_SWING_1,       () => new Sound(load_sfx("melee_swing_1"),      AudioManager.sfx_mixer, 1f, 1.15f, 0.85f)},
        {SoundID.MELEE_SWING_2,       () => new Sound(load_sfx("melee_swing_2"),      AudioManager.sfx_mixer, 1f)},
        {SoundID.MELEE_SWING_3,       () => new Sound(load_sfx("melee_swing_3"),      AudioManager.sfx_mixer, 1f)},
        {SoundID.MAGIC_1,             () => new Sound(load_sfx("magic_1"),            AudioManager.sfx_mixer, 1f)},
        {SoundID.MAGIC_EXPLOSION,     () => new Sound(load_sfx("magic_explosion"),    AudioManager.sfx_mixer, 1f)},
        {SoundID.SNOW_IMPACT_HEAVY,   () => new Sound(load_sfx("snow_impact_heavy"),  AudioManager.sfx_mixer,.6f)},
        {SoundID.SNOW_IMPACT_LIGHT,   () => new Sound(load_sfx("snow_impact_light"),  AudioManager.sfx_mixer, 1f, 1.15f, 0.85f)},
        {SoundID.WHISTLE_LONG,        () => new Sound(load_sfx("whistle_long"),       AudioManager.sfx_mixer, 1f)},
        {SoundID.WHOOSH_1,            () => new Sound(load_sfx("whoosh_1"),           AudioManager.sfx_mixer, 1f, 1.15f, 0.85f)},
        {SoundID.DOG_BARK_1,          () => new Sound(load_sfx("dog_bark_1"),         AudioManager.sfx_mixer, 1f)},
        {SoundID.LEATHER_CONTORT_1,   () => new Sound(load_sfx("leather_contort_1"),  AudioManager.sfx_mixer, 1f)},
        {SoundID.BOW_SHOT,            () => new Sound(load_sfx("bow_shot"),           AudioManager.sfx_mixer, 1f, 1.15f, 0.85f)},
        {SoundID.COIN_TOSS,           () => new Sound(load_sfx("coin_toss"),          AudioManager.sfx_mixer, 1f)},
        {SoundID.RIDER_YELL,          () => new Sound(load_sfx("rider_yell"),         AudioManager.sfx_mixer,.6f)},
        {SoundID.WOODEN_PING,         () => new Sound(load_sfx("wooden_ping"),        AudioManager.sfx_mixer, .7f)},
        {SoundID.DEEP_BOOM,           () => new Sound(load_sfx("deep_boom"),          AudioManager.sfx_mixer, .8f)},
        {SoundID.STEAM,               () => new Sound(load_sfx("steam"),              AudioManager.sfx_mixer, 0.5f, 1f, 0.75f)},
        {SoundID.DOMINE_VOICE_1,      () => new Sound(load_sfx("domine_voice_1"),     AudioManager.sfx_mixer, 1f, .9f, 0.85f)},
        {SoundID.DOMINE_VOICE_2,      () => new Sound(load_sfx("domine_voice_2"),     AudioManager.sfx_mixer, 1f, .9f, 0.85f)},
        {SoundID.DOMINE_VOICE_3,      () => new Sound(load_sfx("domine_voice_3"),     AudioManager.sfx_mixer, 1f, .9f, 0.85f)},
        {SoundID.DOMINE_VOICE_4,      () => new Sound(load_sfx("domine_voice_4"),     AudioManager.sfx_mixer, 1f, .9f, 0.85f)},
        {SoundID.NONE,                () => new Sound(load_sfx("silence"),            AudioManager.sfx_mixer, 1f)},
        {SoundID.SMALL_FIRE,          () => new Sound(load_sfx("small_fire"),         AudioManager.sfx_mixer, .75f, 1, .8f)}
    };
}



