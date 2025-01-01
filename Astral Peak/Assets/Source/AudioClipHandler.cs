using System.Collections;
using UnityEngine;
using Sounds;

public static class AudioClipHandler{
    static MonoBehaviour object_audio;

    static AudioSource create_source(Sound sound, AudioSourceSettings settings){
        AudioSource source = object_audio.gameObject.AddComponent<AudioSource>();
        source.clip                  = sound.clip();
        source.volume                = sound.volume();
        source.pitch                 = sound.max_pitch();
        source.outputAudioMixerGroup = sound.group();

        source.loop                  = settings.loop;
        if(source.loop == false)
            Object.Destroy(source,sound.clip().length); // unscaled time btw
        
        if(settings.randomise_pitch == true)
            source.pitch = sound.randomise_pitch();
        
        if(settings.spatial_blend == false)
            return source;
        
        source.dopplerLevel          = 0;
        source.rolloffMode           = AudioRolloffMode.Linear;
        source.maxDistance           = 48;
        source.spatialBlend          = 1;
        
        return source;          
    }

    public static void set_game_object(MonoBehaviour audio) => object_audio = audio;

    public static AudioSource play(SoundID sound_id, MonoBehaviour audio_player,AudioSourceSettings settings){
        Sound sound = SoundLibrary.get_sound(sound_id);
        set_game_object(audio_player);
        AudioSource source = create_source(sound, settings);
        source.Play();
        return source;
    }

    public static void crossfade(MonoBehaviour audio, ref AudioSource source, SoundID sound_id, float fade_factor, AudioSourceSettings settings){
        fade_out(audio, source, fade_factor);
        source = fade_in(audio, sound_id, fade_factor, settings);
    }

    public static AudioSource fade_in(MonoBehaviour audio_player, SoundID sound_id, float fade_factor, AudioSourceSettings settings){
        Sound sound = SoundLibrary.get_sound(sound_id);
        set_game_object(audio_player);
        AudioSource source = create_source(sound, settings);
        object_audio.StartCoroutine(fade_in_loop(source, fade_factor, sound));
        source.Play();
        return source;
    }

    public static void fade_out(MonoBehaviour audio, AudioSource source, float fade_factor, bool destroy_source = false){
        set_game_object(audio);
        object_audio.StartCoroutine(fade_out_loop(source, fade_factor, destroy_source));
    }

    static IEnumerator fade_in_loop(AudioSource source, float fade_factor, Sound sound){
        if(source == null)
            yield break;
        source.volume = 0;
        while(source.volume < sound.volume()){
            source.volume += Time.deltaTime * fade_factor;
            yield return null;
        }
        source.volume = sound.volume();
        yield break;        
    }

    static IEnumerator fade_out_loop(AudioSource source, float fade_factor, bool destroy_source){
        if(source == null)
            yield break;
        float time_factor = source.volume;
        while(source.volume > 0){
            source.volume -= Time.deltaTime * fade_factor;
            yield return null;
        }
        if(destroy_source == true)
            GameObject.Destroy(source);
        else
            source.clip = null;
        yield break;
    }
}

