using System.Collections;
using UnityEngine;

public class Turret : MonoBehaviour{
    [SerializeField] private float fire_rate = 5.0f;
    [SerializeField] private float fire_rate_timer = 0.0f;
    [SerializeField] private Transform shoot_point;
    [SerializeField] private GameObject projectile;
    private Coroutine coroutine;

    void Start(){
        start_firing();
    }

    public void start_firing(){
        coroutine = StartCoroutine(fire_loop());
    }

    public void stop_firing(){
        StopCoroutine(coroutine);
        // reset the timer so the fire rate is correctly synced when starting again.
        fire_rate_timer = 0.0f;
    }

    IEnumerator fire_loop(){
        while(true){
            if(fire_rate_timer < fire_rate)
                fire_rate_timer += Time.deltaTime;
            else{
                Debug.Log("Fire!");
                Instantiate(projectile, shoot_point.position, shoot_point.rotation);
                fire_rate_timer = 0.0f;
            }  
            yield return null;
        }
    }
}
