using System;
using System.Collections;
using UnityEngine;

public class LineParticleEmitter : MonoBehaviour{
    [SerializeField] public ParticleSystem particles;
    ParticleSystem.EmitParams emission_parameters;
    [SerializeField] Transform start_point, end_point;
    [SerializeField] int particles_per_unit;
    [SerializeField] float unit_length, emision_delay;
    //void Awake() => start_emitting();
    public void set_start_point_position(Vector2 pos) => start_point.position = pos;
    public void set_end_point_position(Vector2 pos) => end_point.position = pos;
    public void start_emitting(){
        StopAllCoroutines();
        StartCoroutine(emission_loop());
        emitted();
        emitting();        
    }
    public void stop_emitting(){
        StopAllCoroutines();
        ended();
    }
    public void emit_once(Vector3 start_pos, Vector3 end_pos){
        emit(particles_per_unit,unit_length, start_pos, end_pos);
        emitted();
    }
    public void emit_once(){
        emit(particles_per_unit,unit_length, null, null);
        emitted();
    }
    protected virtual IEnumerator emission_loop(){
        while(true){
            emit(particles_per_unit:particles_per_unit,unit_length:unit_length, null, null);
            yield return new WaitForSeconds(emision_delay);
            yield return null;
        }
    }

    public ParticleSystem get_particles() => particles;

    protected void emit(int intermediate_particles){
        Vector3 distance = end_point.position - start_point.position;
        float factor = 1/intermediate_particles;
        float x = factor;
        emission_parameters.position = start_point.position;
        particles.Emit(emission_parameters, 1);
        while(x<1){
            emission_parameters.position = start_point.position + distance * x;
            x+=factor;
            particles.Emit(emission_parameters, 1);            
        }
        emission_parameters.position = end_point.position;
        particles.Emit(emission_parameters, 1);
    }
    protected void emit(int particles_per_unit, float unit_length, Vector3? _start_pos, Vector3? _end_pos){
        Vector3 start_pos = _start_pos ?? start_point.position;
        Vector3 end_pos = _end_pos ?? end_point.position;
        // Calculate the length of the line
        Vector3 distance = end_pos - start_pos;
        float line_length = distance.magnitude;
        //float totalParticles = lineLength * particlesPerUnitLength/unit_length;
        int totalParticles = Mathf.FloorToInt(line_length * particles_per_unit / unit_length);
        float factor = 1f / (totalParticles + 1);
        float x = factor;
        // emit start.
        emission_parameters.position = start_pos;
        particles.Emit(emission_parameters, 1);
        // emit intermediate.
        while(x<1){
            emission_parameters.position = start_pos + distance * x;
            x+=factor;
            particles.Emit(emission_parameters, 1);            
        }
        // emit end.
        emission_parameters.position = end_pos;
        particles.Emit(emission_parameters, 1);    
    }

    protected virtual void emitted(){}
    protected virtual void emitting(){}
    protected virtual void ended(){}
    void OnDestroy(){
        ended();
    }
}
