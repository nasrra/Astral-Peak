using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using Sounds;
using Entropek;
using Unity.VisualScripting;

public static class AudioManager{
    static Coroutine 
        filter_state, 
        sfx_volume_state,
        sfx_filter_state,
        music_volume_state,
        music_loop_state,
        music_fade_state,
        ambience_volume_state,
        ambience_fade_state,
        ambience_loop_state;

    public const string
        MUSIC_VOLUME = "MusicVolume",
        SFX_VOLUME = "SfxVolume",
        VOICE_VOLUME = "VoiceVolume";
    public static AudioMixer mixer;
    public static AudioMixerGroup 
        master_mixer,
        music_mixer,
        sfx_mixer,
        voice_mixer; 

    static AudioSource
        current_music, previous_music, current_ambience, previous_ambience;

    static bool reverse_music_crossfade = false, reverse_ambience_crossfade = false;

    public static void on_start() => load_volume_settings();
    public static void initialize(){
        mixer           = Resources.Load<AudioMixer>("Audio/Mixer");
        master_mixer    = mixer.FindMatchingGroups("Master")[0];
        music_mixer     = mixer.FindMatchingGroups("Music")[0];
        sfx_mixer       = mixer.FindMatchingGroups("Sfx")[0];
        voice_mixer     = mixer.FindMatchingGroups("Voice")[0];
        current_music         = UnityHook.instance.AddComponent<AudioSource>();
        previous_music        = UnityHook.instance.AddComponent<AudioSource>();
        current_ambience      = UnityHook.instance.AddComponent<AudioSource>();
        previous_ambience     = UnityHook.instance.AddComponent<AudioSource>();
        current_music    .outputAudioMixerGroup = music_mixer;
        previous_music   .outputAudioMixerGroup = music_mixer;
        current_ambience .outputAudioMixerGroup = sfx_mixer;
        previous_ambience.outputAudioMixerGroup = sfx_mixer;
    }    
    static void state_switch(ref Coroutine coroutine, IEnumerator _coroutine){
        if(coroutine != null)
            UnityHook.instance.StopCoroutine(coroutine);
        coroutine = _coroutine != null? UnityHook.instance.StartCoroutine(_coroutine) : null;
    }





    // Music settings.////
    public static void music_volume(float volume) => mixer.SetFloat(MUSIC_VOLUME,volume);
    public static void play_music(SoundID sound_id){
        state_switch(ref music_loop_state, music_coroutine(sound_id));       
    }
    static IEnumerator music_coroutine(SoundID sound_id){
        float clip_length = SoundLibrary.get_sound(sound_id).clip.length - 3;
        while (true){
            if(reverse_music_crossfade == false)
                state_switch(ref music_fade_state, AudioClipHandler.crossfade(current_music, previous_music, sound_id, 2f));
            else
                state_switch(ref music_fade_state,AudioClipHandler.crossfade(previous_music, current_music, sound_id, 2f));
            reverse_music_crossfade = !reverse_music_crossfade;
            yield return new WaitForSeconds(clip_length);
        }
    }//
    public static void stop_music(){
        // stop music from looping.
        if(music_loop_state!=null)
            UnityHook.instance.StopCoroutine(music_loop_state);
        if(reverse_music_crossfade == false)
            state_switch(ref music_fade_state, AudioClipHandler.fade_out_unscaled(current_music, 1f));
        else
            state_switch(ref music_fade_state, AudioClipHandler.fade_out_unscaled(previous_music, 1f));
    }    


    // SFX Settings
    public static void sfx_volume(float volume) => mixer.SetFloat(SFX_VOLUME,volume);
    public static void dim_sfx_volume(){
        state_switch(ref sfx_volume_state, lerp_value_unscaled(SFX_VOLUME,-80f, 1f));
    }
    public static void restore_sfx_volume() => state_switch(ref sfx_volume_state, lerp_value_unscaled(SFX_VOLUME, load_sfx_volume(), 1f));
    public static void play_ambience(SoundID sound_id){
        state_switch(ref ambience_loop_state, ambience_coroutine(sound_id));       
    }
    static IEnumerator ambience_coroutine(SoundID sound_id){
        float clip_length = SoundLibrary.get_sound(sound_id).clip.length - 2;
        while (true){
            if(reverse_ambience_crossfade == false)
                yield return AudioClipHandler.crossfade(current_ambience, previous_ambience, sound_id, 1f);
            else
                yield return AudioClipHandler.crossfade(previous_ambience, current_ambience, sound_id, 1f);
            reverse_ambience_crossfade = !reverse_ambience_crossfade;
            yield return new WaitForSeconds(clip_length);
        }
    }
    public static void stop_ambience(){
        if(ambience_loop_state!=null)
            UnityHook.instance.StopCoroutine(ambience_loop_state);
        if(reverse_ambience_crossfade == false)
            state_switch(ref ambience_fade_state, AudioClipHandler.fade_out_unscaled(current_ambience, 1f));
        else
            state_switch(ref ambience_fade_state, AudioClipHandler.fade_out_unscaled(previous_ambience, 1f));
    }  





    public static void low_pass_audio(bool x) => state_switch(ref filter_state, lerp_value("LowpassFreq", x==true?800:22000, .5f));   


    // Voice Settings.
    public static void voice_volume(float volume) => mixer.SetFloat(VOICE_VOLUME, volume);

    private static IEnumerator lerp_value(string name, float value, float time){
        mixer.GetFloat(name, out float current_value);
        state_switch(ref filter_state,Calc.lerp_value(
            val =>mixer.SetFloat(name, val),
            current_value,
            value,
            time
        ));
        yield break;
    }

    private static IEnumerator lerp_value_unscaled(string name, float value, float time){
        mixer.GetFloat(name, out float current_value);
        yield return Calc.lerp_value_unscaled(
            val =>{
                mixer.SetFloat(name, val);
            },
            current_value,
            value,
            time
        );
    }

    public static void load_volume_settings(){
        sfx_volume(load_sfx_volume());
        music_volume(load_music_volume());
        voice_volume(load_voice_volume());
    }
    public static float load_sfx_volume()   => PlayerPrefs.GetFloat(SFX_VOLUME,     -10f);
    public static float load_music_volume() => PlayerPrefs.GetFloat(MUSIC_VOLUME,   -10f);
    public static float load_voice_volume() => PlayerPrefs.GetFloat(VOICE_VOLUME,   -10f);

    public static void save_volume_settings(){
        save_sfx_volume();
        save_music_volume();
        save_voice_volume();
    }
    public static void save_sfx_volume()    =>   PlayerPrefs.SetFloat(SFX_VOLUME,   mixer.GetFloat(SFX_VOLUME, out float v)? v : -10f);
    public static void save_music_volume()  => PlayerPrefs.SetFloat(MUSIC_VOLUME, mixer.GetFloat(MUSIC_VOLUME, out float v)? v : -10f);
    public static void save_voice_volume()  => PlayerPrefs.SetFloat(VOICE_VOLUME, mixer.GetFloat(VOICE_VOLUME, out float v)? v : -10f);
}////
