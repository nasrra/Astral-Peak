using System.Collections;
using Entropek;
using UnityEngine;

public class SludgeGeyser : MonoBehaviour{
    [SerializeField] protected BoxCollider2D hurtbox;
    [SerializeField] protected LineRenderer line;
    [SerializeField] protected ParticleSystem start_particle;
    [SerializeField] protected ParticleSystem end_particle;
    [SerializeField] protected AudioPlayer audio_player;

    Coroutine length_state;

    public virtual void turn_on(){
        hurtbox.enabled = true;
        line.enabled = true;
        start_particle.Play();
        end_particle.Play();
        audio_player.play_diegetic_loop("water_rushing");
    }

    public void lerp_length(float _length, float _time){
        if(length_state!=null)
            StopCoroutine(length_state);
        length_state = StartCoroutine(lerp_length_coroutine(_length, _time));
    }

    IEnumerator lerp_length_coroutine(float _length, float _time){
        yield return Calc.lerp_value(val=>update_length(val), _start: line.GetPosition(1).z, _length, _time);
    }

    public virtual void update_length(float _length){
        line.SetPosition(1, new Vector3(0,0,_length));
        hurtbox.size    = new Vector2(hurtbox.size.x,_length);
        hurtbox.offset  = new Vector2(0,_length/2);
        end_particle.transform.position = transform.position + transform.up * _length;
    }

    public void set_sound_intensity(int _intensity){
        audio_player.set_diegetic_instance_parameter("water_rushing", "intensity", _intensity);
    }

    public void play_burst_sound(){
        audio_player.play_diegetic_one_shot("water_gush");
    }

    public virtual void turn_off(){
        hurtbox.enabled = false;
        line.enabled = false;
        end_particle.Stop(false, ParticleSystemStopBehavior.StopEmitting);
        start_particle.Stop(false, ParticleSystemStopBehavior.StopEmitting);
        audio_player.stop_all_loops();
    }
}
