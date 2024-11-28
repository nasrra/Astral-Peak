using UnityEngine;

public class DialogueTrigger : MonoBehaviour{
    [SerializeField] bool loop = false;
    [SerializeField] bool start_trigger = false;
    bool flag = false;
    void OnTriggerEnter2D(Collider2D col){
        if(loop == true || flag == false){
            if(start_trigger == true )
                UiManager.instance.start_dialogue();
            else
                UiManager.instance.next_dialogue_line();
            flag = true;
        }
    }
}
