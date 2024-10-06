using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour{
    [SerializeField] protected Slider left_slider, right_slider;

    public void set_bar_value(float x){
        left_slider.value = x;
        right_slider.value = x;
    }

    public void set_bar_max_value(float x){
        left_slider.maxValue = x;
        right_slider.maxValue = x;
    }

    public float get_max_value() => left_slider.maxValue;
    public float get_current_value() => left_slider.value;
}
