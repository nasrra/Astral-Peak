using UnityEngine;

public class DialogueTrigger : MonoBehaviour{
    [SerializeField] bool loop = false;
    [SerializeField] bool start_trigger = false;
    bool flag = false;
    void OnTriggerEnter2D(Collider2D col){
        if(loop == true || flag == false){
            if(start_trigger == true )
                DialogueHandler.instance.start_dialogue();
            else
                DialogueHandler.instance.next_line();//
            flag = true;
        }
    }
}
