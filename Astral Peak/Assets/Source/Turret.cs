using System.Collections;
using UnityEngine;

public class Turret : MonoBehaviour{
    [SerializeField] private float fire_rate = 5.0f;
    [SerializeField] private Transform shoot_point;
    [SerializeField] private GameObject projectile;
    private Coroutine coroutine;

    public void start_firing() => coroutine = StartCoroutine(fire_loop());
    public void stop_firing() => StopCoroutine(coroutine);

    IEnumerator fire_loop(){
        while(true){
            Debug.Log("Fire!");
            Instantiate(projectile, shoot_point.position, transform.rotation);
            yield return new WaitForSeconds(fire_rate);
        }
    }
}
