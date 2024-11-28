using UnityEngine;

public class AudioReactiveParticleSystem : MonoBehaviour
{
    [SerializeField] AudioSpectrum sound;
    [SerializeField] ParticleSystem particles;
    [SerializeField] float factor = 1;
    void update_light(float magnitude){
        var main = particles.main;
        main.simulationSpeed         = 1 + magnitude * factor;
    }

    // Start is called before the first frame update
    void Start() => sound.magnitude_updated += update_light;
    void OnDestroy() => sound.magnitude_updated -= update_light;
}
