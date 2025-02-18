using Entropek;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour{
    [SerializeField] Slider 
        music_slider,
        sfx_slider,
        ambience_slider,
        voice_slider;
    void OnEnable(){
        music_slider.value      = AudioManager.get_music_volume();
        sfx_slider.value        = AudioManager.get_sfx_volume();
        voice_slider.value      = AudioManager.get_voice_volume();
        ambience_slider.value   = AudioManager.get_ambience_volume();
        music_slider.onValueChanged.AddListener(music_changed);
        sfx_slider.onValueChanged.AddListener(sfx_changed);
        voice_slider.onValueChanged.AddListener(voice_changed);
        ambience_slider.onValueChanged.AddListener(ambience_changed);
    }
    void OnDisable(){
        music_slider.onValueChanged.RemoveListener(music_changed);
        sfx_slider.onValueChanged.RemoveListener(sfx_changed);
        voice_slider.onValueChanged.RemoveListener(voice_changed);
        ambience_slider.onValueChanged.RemoveListener(ambience_changed);
        AudioManager.save_volume_settings();
    }
    void music_changed(float x)     => AudioManager.set_music_volume(x);
    void sfx_changed(float x)       => AudioManager.set_sfx_volume(x);
    void voice_changed(float x)     => AudioManager.set_voice_volume(x);
    void ambience_changed(float x)  => AudioManager.set_ambience_volume(x); 
}
