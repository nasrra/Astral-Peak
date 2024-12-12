using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour{
    [SerializeField] Slider 
        music_slider,
        sfx_slider,
        voice_slider;
    void OnEnable(){
        music_slider.value = PlayerPrefs.GetFloat(AudioManager.MUSIC_VOLUME, 1f);
        music_slider.onValueChanged.AddListener(AudioManager.music_volume);
        sfx_slider.value = PlayerPrefs.GetFloat(AudioManager.SFX_VOLUME, 1f);
        sfx_slider.onValueChanged.AddListener(AudioManager.sfx_volume);
        voice_slider.value = PlayerPrefs.GetFloat(AudioManager.VOICE_VOLUME, 1f);
        voice_slider.onValueChanged.AddListener(AudioManager.voice_volume);
    }
    void OnDisable(){
        music_slider.onValueChanged.RemoveListener(AudioManager.music_volume);
        sfx_slider.onValueChanged.RemoveListener(AudioManager.sfx_volume);
        voice_slider.onValueChanged.RemoveListener(AudioManager.voice_volume);
        PlayerPrefs.SetFloat(AudioManager.MUSIC_VOLUME, music_slider.value);
        PlayerPrefs.SetFloat(AudioManager.SFX_VOLUME, sfx_slider.value);
        PlayerPrefs.SetFloat(AudioManager.VOICE_VOLUME, voice_slider.value);
    }//
}
