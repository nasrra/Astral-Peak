using System;
using System.Collections;
using UnityEngine;

[System.Serializable]
public class RangedHolster{
    public event Action<GameObject> projectile_fired;
    [SerializeField] private bool projectile_is_child = false;
    [SerializeField] private float fire_rate = 5.0f;
    [SerializeField] protected GameObject projectile;
    [SerializeField] protected Transform fire_point;
    private Coroutine coroutine;

    public virtual void fire_once(){
        GameObject x = GameObject.Instantiate(projectile, fire_point.position, fire_point.rotation); // y rotation on z because projectiles rotate on the z axis. 
        x.transform.parent = projectile_is_child == true? fire_point:null;
        invoke_projectile_fired(x); 
    }
    //public void start_firing() => coroutine = StartCoroutine(fire_loop());
    //public void stop_firing() => StopCoroutine(coroutine);
    protected void invoke_projectile_fired(GameObject x) => projectile_fired?.Invoke(x);

    IEnumerator fire_loop(){
        while(true){
            Debug.Log("Fire!");
            fire_once();
            yield return new WaitForSeconds(fire_rate);
        }
    }
}

