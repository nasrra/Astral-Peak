using UnityEngine;

public class CutsceneTrigger : MonoBehaviour{
    [SerializeField] string cutscene;
    [SerializeField] bool flag = false;
    void OnTriggerEnter2D(){
        if(flag == false){
            CutsceneManager.play(cutscene);
            flag = true;
        }
    }

}
