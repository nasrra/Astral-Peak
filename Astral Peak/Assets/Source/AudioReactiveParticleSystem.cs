using System.Collections;
using UnityEngine;
using UnityEngine.TextCore;

public class AudioReactiveParticleSystem : MonoBehaviour
{
    [SerializeField] AudioSpectrum sound;
    [SerializeField] ParticleSystem particles;
    [SerializeField] float factor = 1;
    void update_speed(float magnitude){
        var main = particles.main;
        main.simulationSpeed = 1 + magnitude * factor;
    }

    // Start is called before the first frame update
    void Start() => sound.magnitude_updated += update_speed;
    void OnDestroy() => sound.magnitude_updated -= update_speed;
}
