using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour{
    [SerializeField] protected Slider left_slider, right_slider;
    [SerializeField] protected Health health;

    public void set_bar_value(float x){
        left_slider.value = x;
        right_slider.value = x;
    }

    public void set_bar_max_value(float x){
        left_slider.maxValue = x;
        right_slider.maxValue = x;
    }
}
