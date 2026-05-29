using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DomineDialogueShrineLink : MonoBehaviour{
    [SerializeField] Door shrine_door;
    void handle_new_line(int x){
        switch(x){
            case 27:
                shrine_door.open();
            break;
        }
    }
    void Start(){
        DialogueHandler.instance.new_line += handle_new_line;
    }

    void OnDestroy(){
        DialogueHandler.instance.new_line -= handle_new_line;
    }
}
