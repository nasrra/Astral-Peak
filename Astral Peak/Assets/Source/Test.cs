using System.Collections;
using UnityEngine;

public class Test : MonoBehaviour{
    [SerializeField] float time = 10;
    void Start()=>StartCoroutine(test());
    IEnumerator test(){
        while(true){
            CameraController.instance.lerp_offset(x:null,y:6,time/3);
            CameraController.instance.lerp_regulators(Vector2.zero, null,time/3);
            CameraController.instance.lerp_zoom(16,time/3);
            yield return new WaitForSeconds(time/3);
            CameraController.instance.reset_offset(time/3);
            CameraController.instance.reset_regulators(time/3);
            CameraController.instance.reset_zoom(time/3);
            yield return new WaitForSeconds(time/3);
        }
    }
}
