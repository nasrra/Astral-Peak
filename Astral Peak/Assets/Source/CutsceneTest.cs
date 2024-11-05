using System.Collections;
using UnityEngine;

public class CutsceneTest : Cutscene{
    public override void begin(){
        Player.player.enter_cutscene_state();
        TheCavalry.instance.enter_cutscene_state();
        CutsceneManager.instance.switch_state(test());
    }

    public override void end(){
        Player.player.exit_cutscene_state();
        TheCavalry.instance.exit_cutscene_state();
        Player.exit_point = BossRoomManager.instance.player_respawn_point.get_enter_point();
    }

    public IEnumerator test(){
        Player.player.get_movement().move_left(true);
        yield return new WaitForSeconds(1);
        Player.player.get_movement().move_left(false);
        end();
    }

}
