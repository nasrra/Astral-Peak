using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Audio;

public class AudioClipHandler : MonoBehaviour{
    public static AudioClipHandler instance;
    void Awake(){
        if(instance == null){
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    } 

    public void Play(Sound sound){
        AudioSource s = gameObject.AddComponent<AudioSource>();
        Debug.Log(sound.clip.name);
        s.clip = sound.clip;
        s.volume = sound.volume;
        s.pitch = sound.pitch;
        s.outputAudioMixerGroup = AudioManager.mixer.FindMatchingGroups("Music")[0];
        s.Play();
    }
}
