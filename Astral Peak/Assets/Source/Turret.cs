using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class Turret : MonoBehaviour{
    public event Action<GameObject> projectile_fired;
    [SerializeField] private float fire_rate = 5.0f;
    [SerializeField] protected GameObject projectile;
    private Coroutine coroutine;

    //void Awake() => test();

    public virtual void fire_once(){
        GameObject x = Instantiate(projectile, transform.position, transform.rotation); // y rotation on z because projectiles rotate on the z axis.
        invoke_projectile_fired(x); 
    }
    public void start_firing() => coroutine = StartCoroutine(fire_loop());
    public void stop_firing() => StopCoroutine(coroutine);
    protected void invoke_projectile_fired(GameObject x) => projectile_fired?.Invoke(x);

    IEnumerator fire_loop(){
        while(true){
            Debug.Log("Fire!");
            fire_once();
            yield return new WaitForSeconds(fire_rate);
        }
    }

    void test() => StartCoroutine(fire_loop());
}

