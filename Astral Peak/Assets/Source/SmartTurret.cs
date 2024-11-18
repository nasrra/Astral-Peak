using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// used for turrets with projectiles that have behaviours.
// such as arrows that move down.

public class SmartTurret : Turret{
    public override void fire_once(){
        float y_factor = transform.rotation.eulerAngles.y != 0? 1 : -1;
        float z_rotation = transform.rotation.eulerAngles.z * y_factor;
        GameObject x = Instantiate(projectile, transform.position, Quaternion.Euler(0, 0, transform.rotation.eulerAngles.y - z_rotation)); // y rotation on z because projectiles rotate on the z axis.
        invoke_projectile_fired(x); 
    }
}
