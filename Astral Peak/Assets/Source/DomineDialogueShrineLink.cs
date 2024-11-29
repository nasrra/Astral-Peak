using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DomineDialogueShrineLink : MonoBehaviour{
    [SerializeField] DialogueHandler dialogue;
    [SerializeField] Door shrine_door;
    void handle_new_line(int x){
        switch(x){
            case 27:
                shrine_door.open();
            break;
        }
    }
    void OnEnable(){
        dialogue.new_line += handle_new_line;
    }

    void OnDisable(){
        dialogue.new_line -= handle_new_line;
    }
}
