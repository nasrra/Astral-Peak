using UnityEngine;

public class ShrineCutsceneTrigger : CutsceneTrigger{
    protected override Cutscene get_cutscene()
        => new ShrineOpeningCutscene();
}
