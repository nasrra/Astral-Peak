using UnityEngine;
using System.Collections;

public class ChimeForceApplier : MonoBehaviour{
    [SerializeField] Rigidbody2D chime_end;
    void Awake() => StartCoroutine(loop());

    IEnumerator loop(){
        chime_end.AddForce(new Vector2(Random.Range(5f, 7f),0), ForceMode2D.Impulse);
        yield return new WaitForSeconds(4f);
        yield return null;
    }

}
