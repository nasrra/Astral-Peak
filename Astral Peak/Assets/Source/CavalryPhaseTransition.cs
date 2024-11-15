using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CavalryPhaseTransition : Cutscene{
    public override void begin() => CutsceneManager.set_coroutine(fade());

    IEnumerator fade(){
        Player.player.enter_cutscene_state();
        fade_to_black();
        yield return new WaitForSeconds(1f); 
        CavalryBossRoom.instance.reset_objects.Invoke();
        TheRider.instance.enter_cutscene_state();
        fade_from_black();
        yield return new WaitForSeconds(1f);
        end(); 
        yield break;
    }

    public override void end(){
        Player.player.exit_cutscene_state();
        TheRider.instance.exit_cutscene_state();
    }
}
