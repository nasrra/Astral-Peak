using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour{
    [SerializeField] List<int> line;
    int index = 0;
    bool flag = false;

    void OnTriggerEnter2D(Collider2D col){
        if(flag == false){
            play_dialogue(); // passing zero for no reason.
            flag = true;
        }
    }

    public void play_dialogue(int x = 1){
        if(index < line.Count){
            DialogueHandler.instance.play_line(line[index]);//
            index++;
            DialogueHandler.instance.line_ended -= play_dialogue;
            DialogueHandler.instance.line_ended += play_dialogue;
        }
        else
            DialogueHandler.instance.line_ended -= play_dialogue;
    }
}
