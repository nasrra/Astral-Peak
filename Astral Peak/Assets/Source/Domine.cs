using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Domine : MonoBehaviour{
    [field: SerializeField] public SpriteHandler sprite;
    [field: SerializeField] public ParticleHandler particles {get; private set;}
    [field: SerializeField] public AudioPlayer audio_player {get; private set;}
    [SerializeField] private Material hdr_accents_material;
    public void turn_off_accents(){
        sprite.set_color("_hdr_color", Color.white * -10);
    }
    
    public void flash_on_accents(){
        audio_player.play_non_diegetic_one_shot("deep_gong");
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

    public void flash_off_accents(){
        audio_player.play_non_diegetic_one_shot("deep_gong");
        StartCoroutine(sprite.lerp_color(
            _start: hdr_accents_material.GetColor("_hdr_color"),
            _end: Color.white * 2,
            _value: "_hdr_color",
            _time:  0.5f,
            _callback: ()=>{
                StartCoroutine(sprite.lerp_color(
                    _start: Color.white * 2,
                    _end: Color.white * -10,
                    _value: "_hdr_color",
                    _time: 1f,
                    _callback:()=>{
                        for(int i = 0; i < 6; i++)
                            sprite.enable_sprite($"eye_{i}", false);
                        for(int i = 0; i < 3; i++)
                            sprite.enable_sprite($"star_{i}", false);
                    }
                ));
            }
        ));
    }

    void turn_on_attracted_particles(){
        for(int i = 0; i < 4; i++)
            particles.play_particle($"attracted_{i}");
    }

    void turn_off_attracted_particles(){
        for(int i = 0; i < 4; i++)
            particles.stop_particle($"attracted_{i}");
    }

    public void dissolve(){
        StartCoroutine(dissolve_coroutine());
    }

    IEnumerator dissolve_coroutine(){
        flash_off_accents();
        yield return new WaitForSeconds(2);
        audio_player.play_non_diegetic_loop("domine_dissolve");
        turn_on_attracted_particles();
        StartCoroutine(sprite.lerp_value("_dissolve_amount", .9f, -0.2f, 8f));
        yield return new WaitForSeconds(6);
        audio_player.stop_all_loops();
        particles.stop_all_particles();
        yield return new WaitForSeconds(6.5f);
        gameObject.SetActive(false);
    }

    public void water_rush_camera_shake(){
        CameraController.instance.shake_camera(3.5f, 0.5f, false);
    }

    public void water_gush_camera_shake(){
        CameraController.instance.shake_camera(1f, 0.8f, false);
    }

    public void play_intro_water_rush_sound(){
        audio_player.play_diegetic_loop("water_rushing");
        audio_player.set_diegetic_instance_parameter("water_rushing", "intensity",1);
    }

}
