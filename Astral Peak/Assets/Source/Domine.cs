using UnityEngine;

public class Domine : MonoBehaviour{
    [field: SerializeField] public SpriteHandler sprite;
    [field: SerializeField] public AudioReactiveHDRColor audio_reactive_color {get;private set;}
    [SerializeField] private Material hdr_accents_material;
    public void turn_off_accents(){
        sprite.set_color("_hdr_color", Color.white * -10);
    }
    
    public void flash_accents(){
        StartCoroutine(sprite.lerp_color(
            _start: hdr_accents_material.GetColor("_hdr_color"),
            _end: Color.white * 2,
            _value: "_hdr_color",
            _time:  0.2f,
            _callback: ()=>{
                StartCoroutine(sprite.lerp_color(
                    _start: Color.white * 2,
                    _end: hdr_accents_material.GetColor("_hdr_color"),
                    _value: "_hdr_color",
                    _time: 1f
                ));
            }
        ));
    }
}
