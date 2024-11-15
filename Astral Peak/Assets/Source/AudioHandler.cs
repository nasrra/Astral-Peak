using System.Collections;
using UnityEngine;

public static class AudioClipHandler{

    static ObjectAudio object_audio;

    static AudioSource create_source(Sound sound){
        AudioSource s = object_audio.gameObject.AddComponent<AudioSource>();
        s.clip = sound.clip;
        s.volume = sound.volume;
        s.pitch = sound.pitch;
        s.outputAudioMixerGroup = sound.group;
        return s;        
    }

    public static void set_game_object(ObjectAudio audio) => object_audio = audio;

    public static void play(ObjectAudio audio, Sound sound, out AudioSource source){
        set_game_object(audio);
        source = create_source(sound);
        source.Play();
        Object.Destroy(source,sound.clip.length); // unscaled time btw.
    }

    public static void crossfade(ObjectAudio audio, ref AudioSource source, Sound sound, float fade_factor){
        AudioSource _source;
        fade_out(audio, source, fade_factor);
        fade_in(audio, sound, fade_factor, out _source);
        source = _source;
    }

    public static void fade_in(ObjectAudio audio, Sound sound, float fade_factor, out AudioSource source){
        set_game_object(audio);
        source = create_source(sound);
        object_audio.StartCoroutine(fade_in_loop(source, fade_factor, sound));
        source.Play();
        Object.Destroy(source,sound.clip.length); // unscaled time btw.
    }

    public static void fade_out(ObjectAudio audio, AudioSource source, float fade_factor){
        set_game_object(audio);
        object_audio.StartCoroutine(fade_out_loop(source, fade_factor));
    }

    static IEnumerator fade_in_loop(AudioSource s, float fade_factor, Sound sound){
        s.volume = 0;
        while(s.volume < sound.volume){
            s.volume += Time.deltaTime * fade_factor;
            yield return null;
        }
        s.volume = sound.volume;
        yield break;        
    }

    static IEnumerator fade_out_loop(AudioSource source, float fade_factor){
        while(source.volume > 0){
            source.volume -= Time.deltaTime * fade_factor;
            yield return null;
        }
        source.clip = null;
        //Destroy(source); fix this.
        yield break;
    }
}
