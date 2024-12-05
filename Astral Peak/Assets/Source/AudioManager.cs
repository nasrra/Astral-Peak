using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public static class AudioManager{
    static MonoBehaviour audio_player;
    static Coroutine filter_coroutine, volume_coroutine;

    public const string
        MIXER_MUSIC = "MusicVolume",
        MIXER_SFX = "SfxVolume";
    public static AudioMixer mixer;
    public static AudioMixerGroup 
        master_mixer,
        music_mixer,
        sfx_mixer; 

    static float original_sfx_volume = 0.0f;

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
    
    static void state_switch(ref Coroutine coroutine, IEnumerator _coroutine){
        if(coroutine != null)
            UnityHook.instance.StopCoroutine(coroutine);
        coroutine = UnityHook.instance.StartCoroutine(_coroutine);
    }

    public static void on_start() => load_volume_settings();

    public static void play_music(SoundID sound_id)    => AudioClipHandler.crossfade(audio_player, ref music, sound_id, 1f, AudioSourceSettings.NON_DIEGETIC_LOOP);
    public static void play_ambience(SoundID sound_id) => AudioClipHandler.crossfade(audio_player, ref ambience, sound_id, 1f, AudioSourceSettings.NON_DIEGETIC_LOOP);
    public static void stop_music() => AudioClipHandler.fade_out(audio_player, music, 1f);

    public static void music_volume(float volume) => mixer.SetFloat(MIXER_MUSIC,value_to_logarithmic(volume));
    public static void sfx_volume(float volume) => mixer.SetFloat(MIXER_SFX,value_to_logarithmic(volume));

    public static void low_pass_audio(bool x) => state_switch(ref filter_coroutine, lerp_filter("LowpassFreq", x==true?800:22000, 5));   
    public static void dim_sfx_smooth(){
        float x;
        mixer.GetFloat(MIXER_SFX, out x);
        original_sfx_volume = logarithmic_to_value(x);
        state_switch(ref volume_coroutine, lerp_filter(MIXER_SFX,value_to_logarithmic(0.001f), 1));
    }

    public static void restore_sfx_smooth() => state_switch(ref volume_coroutine, lerp_filter(MIXER_SFX,value_to_logarithmic(original_sfx_volume), 2));

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
    static float logarithmic_to_value(float logarithmicValue) => Mathf.Pow(10, logarithmicValue / 20);


    public static void load_volume_settings(){
        original_sfx_volume = PlayerPrefs.GetFloat(MIXER_SFX, 1f);
        sfx_volume(original_sfx_volume);
        music_volume(PlayerPrefs.GetFloat(MIXER_SFX, 1f));
    }
}
