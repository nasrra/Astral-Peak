using UnityEngine;

public class AudioReactiveHDRColor : MonoBehaviour{
    [SerializeField] AudioSpectrum sound;
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] float hdr_intensity;
    Color color;
    void Awake(){
        color = sprite.material.GetColor("_hdr_color");
        sound.magnitude_updated += update_color;
    }
    public void update_color(float magnitude) => sprite.material.SetColor("_hdr_color", Color.Lerp(color, color * hdr_intensity, magnitude));
    void OnDestroy(){
        sound.magnitude_updated -= update_color;
    } 
}
