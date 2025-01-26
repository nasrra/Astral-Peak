using UnityEngine;
using Sounds;
using Entropek;
using System.Collections;
public static class AudioClipHandler{
    static AudioSource create_source(MonoBehaviour audio_player, Sound sound, AudioSourceSettings settings){
        AudioSource source = audio_player.gameObject.AddComponent<AudioSource>();
        source.clip                  = sound.clip;
        source.volume                = sound.volume;
        source.pitch                 = sound.max_pitch;
        source.outputAudioMixerGroup = sound.group;

        source.loop                  = settings.loop;
        if(source.loop == false)
            Object.Destroy(source,sound.clip.length); // unscaled time btw
        
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

    public static void swap_sources(ref AudioSource source_1, ref AudioSource source_2){
        // Swap sources for tracking in scripts
        AudioSource temp = source_1;
        source_1 = source_2;
        source_2 = temp;        
    }

    public static IEnumerator crossfade(AudioSource source_1, AudioSource source_2, SoundID sound_id, float fade_factor){
        Sound sound = SoundLibrary.get_sound(sound_id);
        source_2.clip = sound.clip;
        source_2.Play();
        yield return Calc.lerp_vector2(
            val =>{
                source_1.volume = val.x; // Fading out
                source_2.volume = val.y; // Fading in
            },
            _start: new Vector2(source_1.volume, 0f), // Start volumes: source_1 at current volume, source_2 at 0
            _end: new Vector2(0f, sound.volume),   // End volumes: source_1 at 0, source_2 at target volume
            _time: fade_factor
        );
    }
    
    public static IEnumerator fade_in(AudioSource source, SoundID sound_id, float time){
        Sound sound = SoundLibrary.get_sound(sound_id);
        yield return fade_in(source, sound, time);
    }

    public static IEnumerator fade_in(AudioSource source, Sound sound, float time){
        source.clip = sound.clip; // Set the sound clip
        source.Play(); // Play the audio source
        yield return Calc.lerp_value(
            _val => source.volume = _val,
            _start: 0,
            _end: sound.volume,
            _time: time
        );
    }

    public static IEnumerator fade_out(AudioSource source, float time, bool destroy_source = false){
        yield return Calc.lerp_value(
            _val=>source.volume=_val,
            _start:source.volume,
            _end:0,
            _time:time,
            _on_complete:()=>{
                if(destroy_source==true)
                    GameObject.Destroy(source);
            }
        );
    }
}

