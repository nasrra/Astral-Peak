using UnityEngine;

public class DialoguePlayer : MonoBehaviour{
    [SerializeField] DialogueHandler dialogue;
    void Start(){
        dialogue.play_dialogue(4f);
    }
}
