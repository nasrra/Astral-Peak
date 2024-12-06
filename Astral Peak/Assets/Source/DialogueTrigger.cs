using UnityEngine;

public class DialogueTrigger : MonoBehaviour{
    [SerializeField] int line = 0;
    bool flag = false;
    void OnTriggerEnter2D(Collider2D col){
        if(flag == false){
            DialogueHandler.instance.play_line(line);//
            flag = true;
        }
    }
}
