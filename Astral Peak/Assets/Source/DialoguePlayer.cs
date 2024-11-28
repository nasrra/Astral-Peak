using UnityEngine;

public class DialoguePlayer : MonoBehaviour{
    [SerializeField] DialogueHandler dialogue;
    void OnDestroy() => dialogue.dialogue_ended -= dialogue_ended;

    void Start(){
        dialogue.play_dialogue();
        //AudioManager.play_music(SoundID.DOMINE_THEME);
        dialogue.dialogue_ended += dialogue_ended;
    }

    void dialogue_ended(){
        dialogue.dialogue_ended -= dialogue_ended;
        AudioManager.stop_music();
    }
}
