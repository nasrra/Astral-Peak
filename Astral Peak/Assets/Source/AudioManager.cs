using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public static class AudioManager{
    static MonoBehaviour audio_player;
    static Coroutine filter_coroutine;

    public const string
        MIXER_MUSIC = "MusicVolume",
        MIXER_SFX = "SfxVolume";
    public static AudioMixer mixer;
    public static AudioMixerGroup 
        master_mixer,
        music_mixer,
        sfx_mixer; 

    static AudioSource
        music, ambience;

    public static void initialize(List<AudioSource> sources, MonoBehaviour _audio_player){
        mixer           = Resources.Load<AudioMixer>("Audio/Mixer");
        master_mixer     = mixer.FindMatchingGroups("Master")[0];
        music_mixer     = mixer.FindMatchingGroups("Music")[0];
        sfx_mixer       = mixer.FindMatchingGroups("Sfx")[0];
        music           = sources[0];
        ambience        = sources[1];
        audio_player    = _audio_player;
    }
    
    public static void on_start() => load_volume_settings();

    public static void play_music(SoundID sound_id)    => AudioClipHandler.crossfade(audio_player, ref music, sound_id, 1f);
    public static void play_ambience(SoundID sound_id) => AudioClipHandler.crossfade(audio_player, ref ambience, sound_id, 1f);
    public static void stop_music() => AudioClipHandler.fade_out(audio_player, music, 1f);

    public static void music_volume(float volume) => mixer.SetFloat(MIXER_MUSIC,value_to_logarithmic(volume));
    public static void sfx_volume(float volume) => mixer.SetFloat(MIXER_SFX,value_to_logarithmic(volume));

    public static void low_pass_audio(bool x){
        if(filter_coroutine != null)
            UnityHook.instance.StopCoroutine(filter_coroutine);
        filter_coroutine = UnityHook.instance.StartCoroutine(lerp_filter("LowpassFreq", x==true?800:22000, 5));   
    }
    static IEnumerator lerp_filter(string name, float value, float speed){
        float x = 0;
        mixer.GetFloat(name, out x);
        while (Mathf.Abs(x - value) > 1f){
            mixer.GetFloat(name, out x);
            mixer.SetFloat(name, Mathf.Lerp(x,value, Time.deltaTime * speed));
            yield return null;
        }
        yield break;
    }

    // calc for mixer because volume levels are set by logarithmic values.
    static float value_to_logarithmic(float value) => Mathf.Log10(value) * 20; 

    public static void load_volume_settings(){
        music_volume(PlayerPrefs.GetFloat(MIXER_MUSIC, 1f));
        sfx_volume(PlayerPrefs.GetFloat(MIXER_SFX, 1f));
    }
}
