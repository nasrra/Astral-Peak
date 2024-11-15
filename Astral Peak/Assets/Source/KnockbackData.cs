using UnityEngine;

[System.Serializable]
public class KnockbackData{
    public KnockbackData(float _force, float _duration, Transform _transform){
        force     = _force;
        duration  = _duration;
        transform = _transform;
    }

    public float 
        force, duration;

    public Transform 
        transform;
}
