using System.Collections;
using Deluz.Collections;
using UnityEngine;

public class Test : MonoBehaviour{
    float time = 6;
    [SerializeField] StateQueue state;
    void Start(){
        state = new StateQueue(this,()=>reset_all());
        StartCoroutine(test());
        state.start();
    }


    void lerp_offset(){
        Debug.Log("offset");
        CameraController.instance.lerp_offset(x:null,y:6,time/2);
    }

    void lerp_zoom(){
        Debug.Log("zoom");
        CameraController.instance.lerp_zoom(16,time/2);
    }

    void reset_all(){
        Debug.Log("reset");
        CameraController.instance.reset_offset(time/2);
        CameraController.instance.reset_zoom(time/2);
    }

    IEnumerator test(){
        while(true){
            state.queue(start_action: lerp_zoom, time_out: null, time: time/2);
            state.queue(start_action: lerp_offset, time_out: null, time: time/2);
            state.state_switch();
            yield return new WaitForSeconds(time*2);
        }
    }
}
