using UnityEngine;
using Sounds;
using Deluz;

public static class AudioClipHandler{
    static AudioSource create_source(MonoBehaviour audio_player, Sound sound, AudioSourceSettings settings){
        AudioSource source = audio_player.gameObject.AddComponent<AudioSource>();
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

    public static AudioSource play(SoundID sound_id,MonoBehaviour audio_player,AudioSourceSettings settings){
        Sound sound = SoundLibrary.get_sound(sound_id);
        AudioSource source = create_source(audio_player, sound, settings);
        source.Play();
        return source;
    }

    public static void crossfade(MonoBehaviour audio_player, ref AudioSource source, SoundID sound_id, float fade_factor, AudioSourceSettings settings){
        fade_out(audio_player, source, fade_factor);
        source = fade_in(audio_player, sound_id, fade_factor, settings);
    }

    public static AudioSource fade_in(MonoBehaviour audio_player, SoundID sound_id, float time, AudioSourceSettings settings){
        Sound sound = SoundLibrary.get_sound(sound_id);
        AudioSource source = create_source(audio_player, sound, settings);
        audio_player.StartCoroutine(Calc.lerp_value(
            _val=>source.volume=_val,
            _start:0,
            _end:sound.volume(),
            _time:time
        ));        
        source.Play();
        return source;
    }

    public static void fade_out(MonoBehaviour audio_player, AudioSource source, float time, bool destroy_source = false)
        =>audio_player.StartCoroutine(Calc.lerp_value(
            _val=>source.volume=_val,
            _start:source.volume,
            _end:0,
            _time:time,
            _on_complete:()=>{
                if(destroy_source==true)
                    GameObject.Destroy(source);
            }
        ));
}

