using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// used for turrets with projectiles that have behaviours.
// such as arrows that move down.

[System.Serializable]
public class SmartTurret : Turret{
    public override void fire_once(){
        float y_factor = fire_point.rotation.eulerAngles.y != 0? 1 : -1;
        float z_rotation = fire_point.rotation.eulerAngles.z * y_factor;
        GameObject x = GameObject.Instantiate(projectile, fire_point.position, Quaternion.Euler(0, 0, fire_point.rotation.eulerAngles.y - z_rotation)); // y rotation on z because projectiles rotate on the z axis.
        invoke_projectile_fired(x); 
    }
}
