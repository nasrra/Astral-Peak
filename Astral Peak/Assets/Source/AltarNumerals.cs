using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class AltarNumerals : MonoBehaviour{
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] ParticleSystem particles;
    [SerializeField] Light2D light2D;
    [SerializeField] int numeral;
    [SerializeField] float light_intensity;
    [SerializeField] float speed;
    void OnEnable() => link();
    void OnDisable() => unlink();
    public void turn_on(){
        particles.gameObject.SetActive(true);
        StartCoroutine(turn_on_light());
        StartCoroutine(turn_on_sprite());
    }
    IEnumerator turn_on_light(){
        AudioClipHandler.play(
            SoundID.DEEP_THUMPING,
            audio_player: this, 
            AudioSourceSettings.DIEGETIC
        );
        while(light2D.intensity < light_intensity){
            light2D.intensity += Time.deltaTime * speed;
            yield return null;
        }
        light2D.intensity = light_intensity;
        yield break;
    }
    IEnumerator turn_on_sprite(){
        while(sprite.color != Color.white){
            sprite.color = Color.Lerp(sprite.color, Color.white, Time.deltaTime * .5f * speed);
            yield return null;
        }
        yield break;
    }

    void handle_cutscene(Cutscene cutscene){
        switch(cutscene){
            case ShrineAltarOneCutscene c:
                switch(numeral){
                    case 1: c.altar_numeral_on += turn_on; break;
                }            
                break;
        }
    }

    void link() => CutsceneManager.started_cutscene += handle_cutscene;
    void unlink() => CutsceneManager.started_cutscene -= handle_cutscene;
}
