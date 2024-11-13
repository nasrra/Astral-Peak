using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour{
    public static AudioManager instance;

    public const string
        MIXER_MUSIC = "MusicVolume",
        MIXER_SFX = "SfxVolume";
    public static AudioMixer mixer;
    public static AudioMixerGroup music_mixer;
    public static AudioMixerGroup sfx_mixer; 

    void Awake(){
        mixer = Resources.Load<AudioMixer>("Audio/Mixer");
        music_mixer = mixer.FindMatchingGroups("Music")[0];
        sfx_mixer = mixer.FindMatchingGroups("Sfx")[0];
    }

    void Start(){load_volume_settings();}
    
    public static void music_volume(float volume) => mixer.SetFloat(MIXER_MUSIC,value_to_logarithmic(volume));
    public static void sfx_volume(float volume) => mixer.SetFloat(MIXER_SFX,value_to_logarithmic(volume));

    // calc for mixer because volume levels are set by logarithmic values.
    static float value_to_logarithmic(float value) => Mathf.Log10(value) * 20; 

    public static void load_volume_settings(){
        music_volume(PlayerPrefs.GetFloat(MIXER_MUSIC, 1f));
        sfx_volume(PlayerPrefs.GetFloat(MIXER_SFX, 1f));
    }
}
