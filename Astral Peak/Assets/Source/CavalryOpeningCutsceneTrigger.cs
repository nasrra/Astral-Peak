using UnityEngine;

public class CavalryOpeningCutsceneTrigger : CutsceneTrigger{
    protected override Cutscene get_cutscene() => new Cutscenes.CavalryOpening();
}
