using System.Collections;
using UnityEngine;

public class LightningController : MonoBehaviour{
    [SerializeField] ParticleSystem particles; 
    [SerializeField] Transform start_point, end_point;
    ParticleSystem.EmitParams emission_parameters;
    void Awake() => StartCoroutine(test());
    public void emit_lightning(){
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
    IEnumerator test(){
        while(true){
            emit_lightning();
            yield return new WaitForSeconds(.5f);
            yield return null;
        }
    }
}
