using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Deluz;

public class SnowController : MonoBehaviour{
    [SerializeField] List<SnowEmitterData> presets;
    [SerializeField] ParticleSystem particle;
    ParticleSystem.MainModule main;
    ParticleSystem.EmissionModule emission;
    ParticleSystem.VelocityOverLifetimeModule velocity; 
    ParticleSystem.MinMaxCurve start_size;    
    Coroutine
        lifetime_state,
        simulation_speed_state,
        min_size_state,
        max_size_state,
        rate_state,
        velocity_state;
    void Awake(){
        get_particle_system_modules();
        StartCoroutine(test());
    }

    void state_switch(ref Coroutine state, IEnumerator _state){
        if(state != null)
            StopCoroutine(state);
        state = StartCoroutine(_state);
    }

    public void set_data(int _preset){
        SnowEmitterData preset = presets[_preset];
        main.startLifetime = preset.lifetime;
        main.simulationSpeed = preset.simulation_speed;
        main.startSize = preset.size;
        emission.rateOverTime = preset.rate;
        velocity.x = preset.velocity.x;
    }

    public void lerp_data(int _preset){
        SnowEmitterData preset = presets[_preset];
        // lifetime.
        state_switch(ref lifetime_state, Calc.lerp_value(val => main.startLifetime = val, main.startLifetime.constant, preset.lifetime, 2));
       
        // speed.
        state_switch(ref simulation_speed_state, Calc.lerp_value(val => main.simulationSpeed = val, main.simulationSpeed, preset.simulation_speed, 2));


       // size.
        state_switch(ref min_size_state, Calc.lerp_value(val =>{
            start_size.constantMin = val;
            main.startSize = start_size; 
        }, main.startSize.constantMin, preset.size.constantMin, 2));
        state_switch(ref max_size_state, Calc.lerp_value(val =>{
            start_size.constantMax = val;
            main.startSize = start_size; 
        }, main.startSize.constantMax, preset.size.constantMax, 2));
        
        // rate.
        state_switch(ref rate_state, Calc.lerp_value(val => emission.rateOverTime = val, emission.rateOverTime.constant, preset.rate, 2));
        state_switch(ref velocity_state, Calc.lerp_value(val => velocity.x = val, velocity.xMultiplier, preset.velocity.x, 2));
    }

    void get_particle_system_modules(){
        main            = particle.main;
        emission        = particle.emission;
        velocity        = particle.velocityOverLifetime;   
        start_size      = main.startSize;
    }

    IEnumerator test(){
        while(true){
            yield return new WaitForSeconds(7);
            lerp_data(1);
            yield return new WaitForSeconds(7);
            lerp_data(0);
            yield return null;            
        }
    }
}

[System.Serializable]
struct SnowEmitterData{
    public float lifetime,rate, simulation_speed;
    public ParticleSystem.MinMaxCurve size;
    public Vector2 velocity;
    public SnowEmitterData(float _lifetime, float _simulation_speed, ParticleSystem.MinMaxCurve _size, float _rate, Vector2 _velocity){
        lifetime = _lifetime;
        simulation_speed = _simulation_speed;
        size = _size;
        rate = _rate;
        velocity = _velocity;
    }
}