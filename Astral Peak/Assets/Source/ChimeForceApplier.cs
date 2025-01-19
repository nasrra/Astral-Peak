using UnityEngine;
using System.Collections;

public class ChimeForceApplier : MonoBehaviour{
    [SerializeField] Rigidbody2D chime_end;
    [SerializeField] float max_force = 7f, min_force = 4f;
    [SerializeField] float max_time = 7, min_time = 4;
    void Awake() => StartCoroutine(loop());

    IEnumerator loop(){
        while(true){
            //chime_end.AddForce(new Vector2(max_force * Mathf.Sin(Time.time),0), ForceMode2D.Force);
            chime_end.AddForce(new Vector2(Random.Range(min_force, max_force),0), ForceMode2D.Impulse);
            yield return new WaitForSeconds(Random.Range(min_time, max_time));
            chime_end.AddForce(new Vector2(Random.Range(-max_force, -min_force),0), ForceMode2D.Impulse);
            yield return new WaitForSeconds(Random.Range(min_time, max_time));
            yield return new WaitForFixedUpdate();
        }//
    }

}
