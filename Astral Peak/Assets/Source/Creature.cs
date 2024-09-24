using Unity.VisualScripting;
using UnityEngine;

public class Creature : MonoBehaviour{
    [Header("Creature")]
    [SerializeField] protected Health health;
    protected virtual void Start(){
        link_events();
    }
    public Health get_health(){
        return health;
    }
    public void Kill(){
        Debug.Log(gameObject.name + " has died!");
        Destroy(gameObject);
    }
    protected virtual void OnDestroy(){
        Debug.Log("s");
        unlink_events();
    }
    protected virtual void link_events(){
        health.on_death += Kill;
    }
    protected virtual void unlink_events(){
        health.on_death -= Kill;
    }
}
