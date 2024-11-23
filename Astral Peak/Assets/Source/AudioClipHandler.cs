using System.Collections;
using UnityEngine;

public static class AudioClipHandler{

    static MonoBehaviour object_audio;

    static AudioSource create_source(Sound sound){
        AudioSource s = object_audio.gameObject.AddComponent<AudioSource>();
        s.clip                  = sound.clip;
        s.volume                = sound.volume;
        s.pitch                 = sound.max_pitch;
        s.outputAudioMixerGroup = sound.group;
        s.dopplerLevel          = 0;
        return s;        
    }

    public static void set_game_object(MonoBehaviour audio) => object_audio = audio;

    public static void play(SoundID sound_id, bool randomise_pitch, bool spatial_blend, MonoBehaviour audio_player, out AudioSource source){
        Sound sound = SoundLibrary.get_sound(sound_id);
        set_game_object(audio_player);
        source = create_source(sound);
        source.pitch = randomise_pitch? sound.randomise_pitch() : source.pitch;
        source.spatialBlend = spatial_blend? 0.7f : 0f;
        source.Play();
        Object.Destroy(source,sound.clip.length); // unscaled time btw.
    }

    public static void crossfade(MonoBehaviour audio, ref AudioSource source, SoundID sound_id, float fade_factor){
        AudioSource _source;
        fade_out(audio, source, fade_factor);
        fade_in(audio, sound_id, fade_factor, out _source);
        source = _source;
    }

    public static void fade_in(MonoBehaviour audio, SoundID sound_id, float fade_factor, out AudioSource source){
        Sound sound = SoundLibrary.get_sound(sound_id);
        set_game_object(audio);
        source = create_source(sound);
        object_audio.StartCoroutine(fade_in_loop(source, fade_factor, sound));
        source.Play();
        Object.Destroy(source,sound.clip.length); // unscaled time btw.
    }

    public static void fade_out(MonoBehaviour audio, AudioSource source, float fade_factor){
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
