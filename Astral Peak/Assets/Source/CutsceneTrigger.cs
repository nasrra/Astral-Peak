using UnityEngine;

public abstract class CutsceneTrigger : MonoBehaviour{
    [SerializeField] bool flag = false;
    void OnTriggerEnter2D(){
        if(flag == false){
            Player.instance.get_movement().zero_velocity();
            Player.instance.transform.position = transform.position;
            CutsceneManager.play(get_cutscene());
            flag = true;
        }
    }
    protected abstract Cutscene get_cutscene();
}
