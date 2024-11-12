using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour{
    [SerializeField] Slider music_slider;
    void Awake() => music_slider.onValueChanged.AddListener(AudioManager.music_volume);
}
