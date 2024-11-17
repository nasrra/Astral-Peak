using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public static class AudioManager{
    static MonoBehaviour audio_player;

    public const string
        MIXER_MUSIC = "MusicVolume",
        MIXER_SFX = "SfxVolume";
    public static AudioMixer mixer;
    public static AudioMixerGroup music_mixer;
    public static AudioMixerGroup sfx_mixer; 

    static AudioSource
        music, ambience;

    public static void initialize(List<AudioSource> sources, MonoBehaviour _audio_player){
        mixer           = Resources.Load<AudioMixer>("Audio/Mixer");
        music_mixer     = mixer.FindMatchingGroups("Music")[0];
        sfx_mixer       = mixer.FindMatchingGroups("Sfx")[0];
        music           = sources[0];
        ambience        = sources[1];
        audio_player    = _audio_player;
    }

    public static void play_music(Sound sound)    => AudioClipHandler.crossfade(audio_player, ref music, sound, 1f);
    public static void play_ambience(Sound sound) => AudioClipHandler.crossfade(audio_player, ref ambience, sound, 1f);
    public static void stop_music() => AudioClipHandler.fade_out(audio_player, music, 1f);

    public static void music_volume(float volume) => mixer.SetFloat(MIXER_MUSIC,value_to_logarithmic(volume));
    public static void sfx_volume(float volume) => mixer.SetFloat(MIXER_SFX,value_to_logarithmic(volume));

    // calc for mixer because volume levels are set by logarithmic values.
    static float value_to_logarithmic(float value) => Mathf.Log10(value) * 20; 

    public static void load_volume_settings(){
        music_volume(PlayerPrefs.GetFloat(MIXER_MUSIC, 1f));
        sfx_volume(PlayerPrefs.GetFloat(MIXER_SFX, 1f));
    }
}
