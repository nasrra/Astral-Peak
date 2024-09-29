using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Creature : MonoBehaviour{
    [Header("Creature")]
    [SerializeField] protected Health health;
    [SerializeField] protected Animator animator;
    void Start(){
        link_events();
    }
    public Health get_health(){
        return health;
    }
    void OnDestroy(){
        unlink_events();
    }

    // flip the sprite.
    protected void flip_left(){
        transform.rotation = Quaternion.Euler(0, 180, 0);
    }

    protected void flip_right(){
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public void kill(){
        Debug.Log(gameObject.name + " has died!");
        Destroy(gameObject);
    }

    protected virtual void link_events(){
        Debug.Log("s");
        health.on_death += kill;
    }

    protected virtual void unlink_events(){
        health.on_death -= kill;
    }
}
