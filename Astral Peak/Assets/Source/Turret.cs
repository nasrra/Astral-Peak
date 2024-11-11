using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class Turret : MonoBehaviour{
    public event Action<GameObject> projectile_fired;
    [SerializeField] private float fire_rate = 5.0f;
    [SerializeField] private GameObject projectile;
    private Coroutine coroutine;

    public void fire_once(){
        GameObject x = Instantiate(projectile, transform.position, transform.rotation);
        projectile_fired?.Invoke(x); 
    }
    public void start_firing() => coroutine = StartCoroutine(fire_loop());
    public void stop_firing() => StopCoroutine(coroutine);

    IEnumerator fire_loop(){
        while(true){
            Debug.Log("Fire!");
            GameObject x = Instantiate(projectile, transform.position, Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z));
            projectile_fired?.Invoke(x); 
            yield return new WaitForSeconds(fire_rate);
        }
    }
}
