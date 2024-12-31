using System;
using UnityEngine;

[System.Serializable]
public class RangedHolster{
    public event Action<GameObject> projectile_fired;
    [SerializeField] private bool projectile_is_child = false;
    [SerializeField] protected GameObject projectile;
    [SerializeField] protected Transform fire_point;
    public virtual void fire_once(){
        GameObject x = GameObject.Instantiate(projectile, fire_point.position, fire_point.rotation); // y rotation on z because projectiles rotate on the z axis. 
        x.transform.parent = projectile_is_child == true? fire_point:null;
        invoke_projectile_fired(x); 
    }
    public virtual void fire_once(Vector2 _fire_point){
        GameObject x = GameObject.Instantiate(projectile, _fire_point, Quaternion.identity); // y rotation on z because projectiles rotate on the z axis. 
        x.transform.parent = projectile_is_child == true? fire_point:null;
        invoke_projectile_fired(x);         
    }
    protected void invoke_projectile_fired(GameObject x) => projectile_fired?.Invoke(x);
}

