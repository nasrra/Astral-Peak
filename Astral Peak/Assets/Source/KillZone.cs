using System.Collections;
using UnityEngine;

public class KillZone : MonoBehaviour{
    void OnTriggerEnter2D(Collider2D other){
        Creature direct = other.GetComponent<Creature>();
        CreatureLink link = other.GetComponent<CreatureLink>();
        Creature creature = direct!=null? direct : link.get_creature();
        if(creature != null){
            if(creature.gameObject.layer == LayersManager.ENEMY)
                creature.kill();
            else if(creature.gameObject.layer == LayersManager.PLAYER){
                creature.health.damage(new DamageData(1), null);
                if(creature.health.get_current_health() >= 0)
                    Player.instance.respawn_state();
            }
        }
    }
}
