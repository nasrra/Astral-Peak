using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour{
    [SerializeField] protected Slider left_slider, right_slider;
    [SerializeField] protected Health health;

    void Start(){
        left_slider.maxValue = health.get_max_guard();
        right_slider.maxValue = health.get_max_guard();
        left_slider.value = health.get_current_guard();
        right_slider.value = health.get_current_guard();
        health.on_guard_update += set_bar_value;
    }

    void set_bar_value(float x){
        left_slider.value = x;
        right_slider.value = x;
    }
}
