using Entropek;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour{
    [SerializeField] Slider 
        music_slider,
        sfx_slider,
        voice_slider;
    void OnEnable(){
        music_slider.value  = AudioManager.get_music_volume();
        sfx_slider.value    = AudioManager.get_sfx_volume();
        voice_slider.value  = AudioManager.get_voice_volume();
        music_slider.onValueChanged.AddListener(music_changed);
        sfx_slider.onValueChanged.AddListener(sfx_changed);
        voice_slider.onValueChanged.AddListener(voice_changed);
    }
    void OnDisable(){
        music_slider.onValueChanged.RemoveListener(music_changed);
        sfx_slider.onValueChanged.RemoveListener(sfx_changed);
        voice_slider.onValueChanged.RemoveListener(voice_changed);
        AudioManager.save_volume_settings();
    }
    void music_changed(float x) => AudioManager.set_music_volume(x);
    void sfx_changed(float x) => AudioManager.set_sfx_volume(x);
    void voice_changed(float x) => AudioManager.set_voice_volume(x);
}
