using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using Sounds;
using Entropek;
using Unity.VisualScripting;

public static class AudioManager{
    static Coroutine 
        filter_state, 
        volume_state,
        music_loop_state,
        music_fade_state,
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
    static float original_sfx_volume = 0.0f;

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
    public static void music_volume(float volume) => mixer.SetFloat(MUSIC_VOLUME,value_to_logarithmic(volume));
    public static void play_music(SoundID sound_id){
        state_switch(ref music_loop_state, music_coroutine(sound_id));       
    }
    static IEnumerator music_coroutine(SoundID sound_id){
        float clip_length = SoundLibrary.get_sound(sound_id).clip().length - 2;
        while (true){
            if(reverse_music_crossfade == false)
                yield return AudioClipHandler.crossfade(current_music, previous_music, sound_id, 1f);
            else
                yield return AudioClipHandler.crossfade(previous_music, current_music, sound_id, 1f);
            reverse_music_crossfade = !reverse_music_crossfade;
            yield return new WaitForSeconds(clip_length);
        }
    }
    public static void stop_music(){
        //state_switch(ref music_state, AudioClipHandler.fade_out(UnityHook.instance, current_music, 2f));
        // stop music from looping.
        Log.MethodCall();
        if(music_loop_state!=null)
            UnityHook.instance.StopCoroutine(music_loop_state);
        state_switch(ref music_fade_state, AudioClipHandler.fade_out(current_music, 2f));
    }    


    public static void play_ambience(SoundID sound_id){
        state_switch(ref ambience_loop_state, ambience_coroutine(sound_id));       
    }
    static IEnumerator ambience_coroutine(SoundID sound_id){
        float clip_length = SoundLibrary.get_sound(sound_id).clip().length - 2;
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
        Log.MethodCall();
        if(ambience_loop_state!=null)
            UnityHook.instance.StopCoroutine(ambience_loop_state);
        if(reverse_ambience_crossfade == false)
            state_switch(ref ambience_fade_state, AudioClipHandler.fade_out(current_ambience, 2f));
        else
            state_switch(ref ambience_fade_state, AudioClipHandler.fade_out(previous_ambience, 2f));
    }  



    public static void sfx_volume(float volume) => mixer.SetFloat(SFX_VOLUME,value_to_logarithmic(volume));


    public static void low_pass_audio(bool x) => state_switch(ref filter_state, lerp_filter("LowpassFreq", x==true?800:22000, 5));   
    public static void dim_sfx_smooth(){
        float x;
        mixer.GetFloat(SFX_VOLUME, out x);
        original_sfx_volume = logarithmic_to_value(x);
        state_switch(ref volume_state, lerp_filter(SFX_VOLUME,value_to_logarithmic(0.001f), 1));
    }

    public static void restore_sfx_smooth() => state_switch(ref volume_state, lerp_filter(SFX_VOLUME,value_to_logarithmic(original_sfx_volume), 2));
    // Voice Settings.
    public static void voice_volume(float volume) => mixer.SetFloat(VOICE_VOLUME, value_to_logarithmic(volume));

    static IEnumerator lerp_filter(string name, float value, float speed){
        float x = 0;
        mixer.GetFloat(name, out x);
        while (Mathf.Abs(x - value) > 1f){
            mixer.GetFloat(name, out x);
            mixer.SetFloat(name, Mathf.Lerp(x,value, Time.deltaTime * speed));
            yield return null;
        }
        mixer.SetFloat(name, value);
        yield break;
    }

    // calc for mixer because volume levels are set by logarithmic values.
    static float value_to_logarithmic(float value) => Mathf.Log10(value) * 20; 
    static float logarithmic_to_value(float logarithmicValue) => Mathf.Pow(10, logarithmicValue / 20);


    public static void load_volume_settings(){
        original_sfx_volume = PlayerPrefs.GetFloat(SFX_VOLUME, 1f);
        sfx_volume(original_sfx_volume);
        music_volume(PlayerPrefs.GetFloat(MUSIC_VOLUME, 1f));
        voice_volume(PlayerPrefs.GetFloat(VOICE_VOLUME, 1f));
    }
}//
