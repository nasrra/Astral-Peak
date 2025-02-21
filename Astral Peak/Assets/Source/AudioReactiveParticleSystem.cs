using System.Collections;
using UnityEngine;
using UnityEngine.TextCore;

public class AudioReactiveParticleSystem : MonoBehaviour{
    [SerializeField] AudioSpectrum audio_spectrum;
    [SerializeField] ParticleSystem particles;
    [SerializeField] float factor = 1;
    void update_speed(float magnitude){
        var main = particles.main;
        main.simulationSpeed = 1 + magnitude * factor;
    }

    // Start is called before the first frame update
    void Start() => audio_spectrum.magnitude_updated += update_speed;
    void OnDestroy() => audio_spectrum.magnitude_updated -= update_speed;
}
////