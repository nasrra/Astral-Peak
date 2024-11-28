using UnityEngine;
using UnityEngine.Rendering.Universal;

public class AudioReactiveLight2D : MonoBehaviour{
    [SerializeField] AudioSpectrum sound;
    [SerializeField] Light2D light2D;
    [SerializeField] float intensity_factor, fall_off_factor;
    float original_intensity;
    float original_fall_off;

    void update_light(float magnitude){
        light2D.intensity           = original_intensity + magnitude * intensity_factor;
        light2D.shapeLightFalloffSize    = original_fall_off + magnitude * fall_off_factor;
    }

    // Start is called before the first frame update
    void Start(){
        original_intensity = light2D.intensity;
        original_fall_off = light2D.shapeLightFalloffSize;
        sound.magnitude_updated += update_light;
    }
    void OnDestroy(){
        sound.magnitude_updated -= update_light;
    }
}
