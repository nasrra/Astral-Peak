using UnityEngine;

public class HurtZone : MonoBehaviour{
    [SerializeField] DamageData damage;
    [SerializeField] KnockbackData knockback;
    void OnTriggerEnter2D(Collider2D other){
        Creature direct = other.GetComponent<Creature>();
        CreatureLink link = other.GetComponent<CreatureLink>();
        Creature creature = direct!=null? direct : link.get_creature();
        if(creature != null && creature.gameObject.layer == LayersManager.PLAYER){
            creature.health.damage(damage, knockback);
        }
    }
}
