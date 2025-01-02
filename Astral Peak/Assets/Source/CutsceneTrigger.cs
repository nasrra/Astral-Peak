using UnityEngine;

public abstract class CutsceneTrigger : MonoBehaviour{
    [SerializeField] bool flag = false;
    void OnTriggerEnter2D(){
        if(flag == false){
            CutsceneManager.play(get_cutscene());
            flag = true;
        }
    }
    protected abstract Cutscene get_cutscene();
}
