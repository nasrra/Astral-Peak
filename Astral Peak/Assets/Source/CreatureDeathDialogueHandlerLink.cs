using UnityEngine;

public class CreatureDeathDialogueHandlerLink : MonoBehaviour{
    [SerializeField] Creature creature;//
    [SerializeField] DialogueTrigger dialogue_trigger;
    void play_dialogue() => dialogue_trigger.play_dialogue();
    void Awake() => creature.death_completed += play_dialogue;
    void OnDestroy() => creature.death_completed -= play_dialogue;
}//
