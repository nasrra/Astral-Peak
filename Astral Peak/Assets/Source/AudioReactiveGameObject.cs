using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioReactiveGameObject : MonoBehaviour{
    [SerializeField] AudioSpectrum sound;
    [SerializeField] float factor;
    Vector3 size;

    void update_light(float magnitude){
        transform.localScale = new Vector3(
            size.x + magnitude * factor,
            size.y + magnitude * factor,
            0);
    }

    // Start is called before the first frame update
    void Start(){
        sound.magnitude_updated += update_light;
        size = gameObject.transform.localScale;
    }
    void OnDestroy(){
        sound.magnitude_updated -= update_light;
    }
}
