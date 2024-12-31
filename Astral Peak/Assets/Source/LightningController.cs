using System.Collections;
using UnityEngine;

public class LightningController : MonoBehaviour{
    [SerializeField] Transform start_point, end_point;
    [SerializeField] float strike_rate = .5f;
    [SerializeField] ParticleSystem particles; 
    ParticleSystem.EmitParams emission_parameters;
    public void start_emitting(){
        StopAllCoroutines();
        StartCoroutine(emission_loop());
    }
    public void emit_once() => particle_emission();
    public void stop_emitting(){
        StopAllCoroutines();
    }
    public void set_start_point_position(Vector2 pos) => start_point.position = pos;
    public void set_end_point_position(Vector2 pos) => end_point.position = pos;
    void particle_emission(){
        Vector3 direction = end_point.position - start_point.position;
        emission_parameters.position = start_point.position;
        particles.Emit(emission_parameters, 1);
        emission_parameters.position = start_point.position + direction * 0.25f;
        particles.Emit(emission_parameters, 1);
        emission_parameters.position = start_point.position + direction * 0.5f;
        particles.Emit(emission_parameters, 1);
        emission_parameters.position = start_point.position + direction * 0.75f;
        particles.Emit(emission_parameters, 1);
        emission_parameters.position = end_point.position;
        particles.Emit(emission_parameters, 1);
    }
    IEnumerator emission_loop(){
        while(true){
            particle_emission();
            yield return new WaitForSeconds(strike_rate);
            yield return null;
        }
    }
}
