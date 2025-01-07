using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.AI;

public class KillZone : MonoBehaviour{
    void OnTriggerEnter2D(Collider2D other){
        Creature direct = other.GetComponent<Creature>();
        CreatureLink link = other.GetComponent<CreatureLink>();
        Creature creature = direct!=null? direct : link.get_creature();
        if(creature != null){
            if(creature.gameObject.layer == LayersManager.ENEMY)
                creature.kill();
            else if(creature.gameObject.layer == LayersManager.PLAYER){
                creature.get_health().damage(new DamageData(1), null);
                if(creature.get_health().get_current_health() <= 0)
                    Player.instance.enter_cutscene_state();
                else
                    StartCoroutine(hit_player());
            }
        }
    }

    // respawn state.
    IEnumerator hit_player(){
        CameraEffects.instance.fade_to_black(1);
        Player.instance.enter_cutscene_state();
        CameraController.instance.stop_follow_state();
        yield return new WaitForSeconds(1);
        Player.instance.set_enter_position();
        CameraController.instance.start_follow_state();
        CameraController.instance.snap_to_target(); 
        CameraEffects.instance.fade_from_black(1);
        CameraController.instance.reset_offset(1);
        yield return new WaitForSeconds(1);
        Player.instance.exit_cutscene_state();
    }
}
