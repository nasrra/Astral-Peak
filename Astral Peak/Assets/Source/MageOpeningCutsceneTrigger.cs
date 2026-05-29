using UnityEngine;
public class MageOpeningCutsceneTrigger : CutsceneTrigger{
    protected override Cutscene get_cutscene() => new Cutscenes.MageOpening();
}
