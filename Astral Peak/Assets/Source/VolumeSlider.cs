using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour{
    [SerializeField] Slider music_slider;
    void Awake(){
        music_slider.value = PlayerPrefs.GetFloat(AudioManager.MIXER_MUSIC, 1f);
        music_slider.onValueChanged.AddListener(AudioManager.music_volume);
    }
    void OnDisable(){
        PlayerPrefs.SetFloat(AudioManager.MIXER_MUSIC, music_slider.value);
    }
}
