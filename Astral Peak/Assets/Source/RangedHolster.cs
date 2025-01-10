using System;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Wordprocessing;
using UnityEngine;

[System.Serializable]
public class RangedHolster{
    public event Action<GameObject> projectile_fired;
    [SerializeField] private RotationType rotation_type = RotationType.Y_TO_Z;
    [SerializeField] private bool projectile_is_child = false;
    [SerializeField] protected GameObject projectile;
    [SerializeField] protected Transform fire_point;
    public virtual void fire_once(){
        GameObject x = GameObject.Instantiate(
            projectile, 
            fire_point.position, 
            fire_point.rotation = rotation_type switch{
                RotationType.Y_TO_Z => Quaternion.Euler(new Vector3(0, 0, fire_point.rotation.eulerAngles.y)),
                _ => fire_point.rotation
            });
            //rotation_type == RotationType.Y
            //    ?fire_point.rotation
            //    :Quaternion.Euler(new Vector3(0,0,fire_point.rotation.eulerAngles.y))); // y rotation on z because projectiles rotate on the z axis. 
        x.transform.parent = projectile_is_child == true? fire_point:null;
        invoke_projectile_fired(x); 
    }
    public virtual void fire_once(Vector2 _fire_point){
        GameObject x = GameObject.Instantiate(projectile, _fire_point, Quaternion.identity); // y rotation on z because projectiles rotate on the z axis. 
        x.transform.parent = projectile_is_child == true? fire_point:null;
        invoke_projectile_fired(x);         
    }
    public void set_fire_point(Transform _fire_point) => fire_point = _fire_point;
    protected void invoke_projectile_fired(GameObject x) => projectile_fired?.Invoke(x);
    enum RotationType{
        Y_TO_Z,INHERIT
    }
}

