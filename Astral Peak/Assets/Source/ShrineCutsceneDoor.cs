using UnityEngine;

public class ShrineCutsceneDoor : MonoBehaviour{
    [SerializeField] Door door;
    void Awake() => link();
    void OnDestroy() => unlink();

    void handle_cutscene(Cutscene cutscene){
        switch(cutscene){
            case ShrineOpeningCutscene c:
                c.open_shrine_door += door.open;
            break;
        }
    }

    void link() => CutsceneManager.started_cutscene += handle_cutscene;
    void unlink() => CutsceneManager.started_cutscene -= handle_cutscene;
}
