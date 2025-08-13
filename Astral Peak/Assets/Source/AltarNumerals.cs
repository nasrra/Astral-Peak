using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Collections.Generic;
using Entropek;

public class AltarNumerals : MonoBehaviour{
    [SerializeField] List<SpriteRenderer> sprite = new List<SpriteRenderer>();
    [SerializeField] List<ParticleSystem> particles = new List<ParticleSystem>();
    [SerializeField] List<Light2D> light2D = new List<Light2D>();
    [SerializeField] List<float> light_intensity = new List<float>();
    [SerializeField] AudioPlayer audio_player;
    void Awake(){
        handle_cutscene();
        set_all_on();
    }

    public void turn_on(List<int> numerals){
        if(numerals==null)
            return;
        foreach(int numeral in numerals)
            turn_on(numeral);
    }
    public void turn_on(int i){
        particles[i].gameObject.SetActive(true);
        audio_player.play_non_diegetic_one_shot("heart_thump");
        StartCoroutine(Calc.lerp_value(
            val=>light2D[i].intensity=val,
            _start: 0,
            _end:   light_intensity[i],
            _time:  1
        ));
        StartCoroutine(Calc.lerp_color(
            color=>sprite[i].color=color,
            _start:new Color(0,0,0,0),
            _end: Color.white,
            _time: 1
        ));
    }
    public void set_on(List<int> numerals){
        if(numerals==null)
            return;
        foreach(int numeral in numerals)
            set_on(numeral);
    }
    public void set_on(int i){
        particles[i].gameObject.SetActive(true);
        light2D[i].intensity = light_intensity[i];
        sprite[i].color = Color.white;
    }
    public void set_all_on(){
        for(int i = 0; i < 3; ++i){
            particles[i].gameObject.SetActive(true);
            light2D[i].intensity = light_intensity[i];
            sprite[i].color = Color.white;
        }
    }
    void handle_cutscene(){
        Cutscene cutscene = CutsceneManager.get_cutscene();
        if(cutscene == null)
            return;
        switch(cutscene){
            case ShrineAltarCutscene c:
                c.turn_on_numeral += turn_on;
                c.set_numerals += set_on;
                break;
            case Cutscenes.DomineDoorFinal c:
                c.numerals_on += set_all_on;
                break;
        }
    }
}
