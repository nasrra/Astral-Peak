using Entropek;
using UnityEngine;

public class GameEndStingerController : MonoBehaviour{
    void Start(){
        StartCoroutine(Util.timer(
            6,
            time_out:()=>CustomSceneManager.load_scene_with_transitions("MainMenu")
        ));      
    }
}
