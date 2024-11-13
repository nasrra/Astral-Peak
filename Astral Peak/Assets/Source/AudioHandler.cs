using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Audio;

public class AudioClipHandler : MonoBehaviour{
    protected float fade_factor = 1f;

    protected IEnumerator sound_lifetime(AudioSource source, float lifetime){
        yield return new WaitForSeconds(lifetime);
        if(source != null)
            Destroy(source);
        yield break;
    }

    AudioSource create_source(Sound sound){
        AudioSource s = gameObject.AddComponent<AudioSource>();
        s.clip = sound.clip;
        s.volume = sound.volume;
        s.pitch = sound.pitch;
        s.outputAudioMixerGroup = sound.group;
        return s;        
    }

    protected void play(Sound sound, out AudioSource source){
        source = create_source(sound);
        StartCoroutine(sound_lifetime(source,sound.clip.length));
        source.Play();
    }

    protected void fade_in(Sound sound, out AudioSource source){
        source = create_source(sound);
        StartCoroutine(fade_in_loop(source,sound));
        StartCoroutine(sound_lifetime(source,sound.clip.length));
        source.Play();
    }

    public void fade_out(AudioSource source) => StartCoroutine(fade_out_loop(source));

    IEnumerator fade_in_loop(AudioSource s, Sound sound){
        s.volume = 0;
        while(s.volume < sound.volume){
            s.volume += Time.deltaTime * fade_factor;
            yield return null;
        }
        s.volume = sound.volume;
        yield break;        
    }

    IEnumerator fade_out_loop(AudioSource source){
        while(source.volume > 0){
            source.volume -= Time.deltaTime * fade_factor;
            yield return null;
        }
        Destroy(source);
        yield break;
    }
}
