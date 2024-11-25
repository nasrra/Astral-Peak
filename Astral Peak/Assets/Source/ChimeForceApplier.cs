using UnityEngine;
using System.Collections;

public class ChimeForceApplier : MonoBehaviour{
    [SerializeField] Rigidbody2D chime_end;
    void Awake() => StartCoroutine(loop());

    IEnumerator loop(){
        chime_end.AddForce(new Vector2(Random.Range(10f, 15f),0), ForceMode2D.Impulse);
        yield return new WaitForSeconds(2);
        yield return null;
    }

}
