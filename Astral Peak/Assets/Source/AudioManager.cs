using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public static class AudioManager{
    static ObjectAudio audio_player;

    public const string
        MIXER_MUSIC = "MusicVolume",
        MIXER_SFX = "SfxVolume";
    public static AudioMixer mixer;
    public static AudioMixerGroup music_mixer;
    public static AudioMixerGroup sfx_mixer; 

    static AudioSource
        music_1, music_2,
        ambience_1, ambience_2;

    public static void initialize(List<AudioSource> sources){
        mixer           = Resources.Load<AudioMixer>("Audio/Mixer");
        music_mixer     = mixer.FindMatchingGroups("Music")[0];
        sfx_mixer       = mixer.FindMatchingGroups("Sfx")[0];
        music_1         = sources[0];
        music_2         = sources[1];
        ambience_1      = sources[2];
        ambience_2      = sources[3];
        audio_player    = StaticComponents.main.AddComponent<ObjectAudio>();
    }
    
    public static void play_music(Sound sound)    => AudioClipHandler.dual_fade(audio_player, music_1, music_2, sound, 1f);
    public static void play_ambience(Sound sound) => AudioClipHandler.dual_fade(audio_player, ambience_1, ambience_2, sound, 1f);
    public static void stop_music(){
        AudioClipHandler.fade_out(audio_player, music_1, 1f);
        AudioClipHandler.fade_out(audio_player, music_2, 1f);
    }
    public static void music_volume(float volume) => mixer.SetFloat(MIXER_MUSIC,value_to_logarithmic(volume));
    public static void sfx_volume(float volume) => mixer.SetFloat(MIXER_SFX,value_to_logarithmic(volume));

    // calc for mixer because volume levels are set by logarithmic values.
    static float value_to_logarithmic(float value) => Mathf.Log10(value) * 20; 

    public static void load_volume_settings(){
        music_volume(PlayerPrefs.GetFloat(MIXER_MUSIC, 1f));
        sfx_volume(PlayerPrefs.GetFloat(MIXER_SFX, 1f));
    }
}
