using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneTest : Cutscene
{
    public override void begin(){
        Player.player.enter_cutscene_state();
        TheCavalry.instance.enter_cutscene_state();
        CutsceneManager.instance.switch_state(test());
    }

    public override void end(){
        Player.player.exit_cutscene_state();
        TheCavalry.instance.exit_cutscene_state();     
    }

    public IEnumerator test(){
        Debug.Log("Test Cutscene!");
        yield return new WaitForSeconds(2);
        Debug.Log("ENDED!");
        end();
    }

}
